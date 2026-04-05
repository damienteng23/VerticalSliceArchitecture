using Dashboard.Application.Slice.Backtesting;
using Dashboard.Application.Slice.ExternalData;
using DbUp;
using Microsoft.Data.SqlClient;
using Refit;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<DashboardDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DashboardDb");
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly("Dashboard.Application");
        sqlServerOptions.CommandTimeout(20);
    });
});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod()
                 .AllowAnyHeader()
                 .WithOrigins("http://localhost:3000")
                 .AllowCredentials();
    });
});
builder.Services.AddRefitClient<IPolygonApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.polygon.io"));

// for global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

#region AddScoped region for repository
builder.Services.AddScoped<IDailyTickerRepository, DailyTickerRepository>();
#endregion

#region AddScoped region for domain services
// builder.Services.AddScoped<IMockDataDomainService, MockDataDomainService>();
#endregion

#region AddScoped region for application services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IExternalDataService, ExternalDataService>();
builder.Services.AddScoped<IBacktestingService, BacktestingService>();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline

// Run migrations in ALL environments with safety checks
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    var connectionString = config.GetConnectionString("DashboardDb");

    // Safety Check 1: Verify connection string exists
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DashboardDb' is not configured.");
    }

    logger.LogInformation("Starting database migration process...");

    // Safety Check 2: Verify database connection
    logger.LogInformation("Verifying database connection...");
    using (var connection = new SqlConnection(connectionString))
    {
        try
        {
            await connection.OpenAsync();
            logger.LogInformation("✓ Database connection verified successfully");
        }
        catch (SqlException sqlEx)
        {
            logger.LogError(sqlEx, "✗ Failed to connect to database. Check connection string and database availability.");
            throw;
        }
    }

    // EF Core migrations (tables, schemas)
    logger.LogInformation("Running EF Core migrations...");
    var dbContext = services.GetRequiredService<DashboardDbContext>();
    await dbContext.Database.MigrateAsync();
    logger.LogInformation("✓ EF Core Schema Migration complete");

    // DbUp migrations (SPs, UDTTs, functions)
    logger.LogInformation("Running DbUp migrations for programmable objects...");
    var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Program).Assembly,
                name => name.Contains("Scripts"))
            .JournalToSqlTable("dbo", "SchemaVersions_DbUp")
            .LogToConsole()
            .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        logger.LogError(result.Error, "✗ DbUp migration failed");
        throw new InvalidOperationException("Database programmable objects migration failed.", result.Error);
    }

    logger.LogInformation("✓ DbUp programmable objects updated successfully");
    logger.LogInformation("✓ All database migrations completed successfully");
}
catch (Exception ex)
{
    logger.LogError(ex, "✗ CRITICAL: Database migration failed. Application cannot start.");
    throw;
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

// Global exception handling
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();

/* .net 6 onwards now allows integration of startup into program.cs! */