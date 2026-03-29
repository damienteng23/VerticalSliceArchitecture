var builder = DistributedApplication.CreateBuilder(args);

// 1. Setup the SQL Server Container
var sql = builder.AddSqlServer("sqlserver"); // resource name
var db = sql.AddDatabase("DashboardDb"); //  matches builder.AddSqlServerDbContext("DashboardDb")

// 2. Reference your Web Project
builder.AddProject<Projects.Dashboard_Application>("api")
       .WithReference(db); // This injects the connection info automatically

builder.Build().Run();