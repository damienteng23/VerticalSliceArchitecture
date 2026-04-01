using Dashboard.Application.Slice.Backtesting;
using Dashboard.Application.Slice.ExternalData;
using DbUp;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

#region Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<DashboardDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DashboardDb");
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly("Dashboard.Application");
        sqlServerOptions.CommandTimeout(60);
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

// for global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

#region AddScoped region for repository
//builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
#endregion

#region AddScoped region for domain services
// builder.Services.AddScoped<IMockDataDomainService, MockDataDomainService>();
#endregion

#region AddScoped region for application services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IExternalDataService, ExternalDataService>();
builder.Services.AddScoped<IBacktestingService, BacktestingService>();
#endregion

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<DashboardDbContext>();
        await dbContext.Database.MigrateAsync();
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("EF Core Schema Migration complete");

        var connectionString = config.GetConnectionString("DashboardDb");

        var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(Program).Assembly,
                    name => name.Contains("Scripts.ProgrammableObjects")) // ONLY run SPs/UDTTs
                .JournalToSqlTable("dbo", "SchemaVersions_DbUp") // Separate journal for DbUp
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            logger.LogError(result.Error, "DbUp migration failed.");
        else
            logger.LogInformation("DbUp programmable objects updated successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }

    app.MapOpenApi();
    app.MapScalarApiReference();

    // Automatically redirect to Scalar documentation
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

// for global exception handling - part 2
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