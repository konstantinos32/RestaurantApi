using RestaurantApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();

// Register the database context and configure SQLite as the database provider.
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("RestaurantDb")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<RestaurantDbContext>();

    await dbContext.Database.MigrateAsync();

    await DbSeeder.SeedAsync(dbContext);
}

app.MapGet("/", () => "Hello World!");

app.MapMenuEndpoints();
app.MapOrderEndpoints();
app.MapReportEndpoints();
app.Run();