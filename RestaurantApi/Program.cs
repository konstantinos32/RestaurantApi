using RestaurantApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Errors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
// Register standardized error responses and our global exception handler.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Register the database context and configure SQLite as the database provider.
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("RestaurantDb")
    )
);

var app = builder.Build();
app.UseExceptionHandler();

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