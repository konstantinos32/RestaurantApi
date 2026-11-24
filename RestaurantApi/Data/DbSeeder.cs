using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;

namespace RestaurantApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(RestaurantDbContext dbContext)
    {
        // Avoid inserting duplicate sample data on every application start.
        if (await dbContext.MenuItems.AnyAsync())
        {
            return;
        }

        var menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Name = "Margherita",
                Description = "Tomato, mozzarella and fresh basil",
                Price = 12.50m,
                Category = "Pizza"
            },
            new MenuItem
            {
                Name = "Pepperoni",
                Description = "Tomato, mozzarella and pepperoni",
                Price = 14.50m,
                Category = "Pizza"
            },
            new MenuItem
            {
                Name = "Caesar Salad",
                Description = "Romaine lettuce, parmesan and croutons",
                Price = 10.00m,
                Category = "Salad"
            },
            new MenuItem
            {
                Name = "Greek Salad",
                Description = "Tomatoes, cucumber, feta and olives",
                Price = 9.50m,
                Category = "Salad"
            },
            new MenuItem
            {
                Name = "Spaghetti Bolognese",
                Description = "Spaghetti with beef and tomato sauce",
                Price = 15.00m,
                Category = "Pasta"
            },
            new MenuItem
            {
                Name = "Carbonara",
                Description = "Spaghetti with egg, parmesan and pancetta",
                Price = 14.00m,
                Category = "Pasta"
            },
            new MenuItem
            {
                Name = "Tiramisu",
                Description = "Classic Italian coffee dessert",
                Price = 6.50m,
                Category = "Dessert"
            },
            new MenuItem
            {
                Name = "Chocolate Cake",
                Description = "Chocolate cake with chocolate cream",
                Price = 6.00m,
                Category = "Dessert"
            }
        };

        await dbContext.MenuItems.AddRangeAsync(menuItems);
        await dbContext.SaveChangesAsync();
    }
}