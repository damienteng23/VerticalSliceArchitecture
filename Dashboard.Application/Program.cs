var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

#region Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();


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

#region Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    //var dbContext = services.GetRequiredService<StoreDbContext>();
    //dbContext.Database.Migrate();

    app.MapOpenApi();
    app.MapScalarApiReference();

    // Automatically redirect to Scalar documentation
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

// for global exception handling - part 2
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
#endregion

app.Run();

/* .net 6 onwards now allows integration of startup into program.cs! */