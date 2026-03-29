var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// required to add .NET aspire to the existing service collection for the application to work
builder.AddServiceDefaults();

#region Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.AddSqlServerDbContext<DashboardDbContext>("DashboardDb",
    settings => { /* You can tune settings here if needed */ },
    options => {
        options.UseSqlServer(sqlServerOptions => {  
            sqlServerOptions.MigrationsAssembly("Dashboard.Application");
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
// builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
#endregion

#region AddScoped region for domain services
// builder.Services.AddScoped<IMockDataDomainService, MockDataDomainService>();
#endregion

#region AddScoped region for application services
// builder.Services.AddScoped<IMockDataService, MockDataService>();
#endregion

#endregion

var app = builder.Build();

// Map the health check endpoints for the Aspire Dashboard
app.MapDefaultEndpoints();

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
        logger.LogInformation("Database migration completed successfully.");
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