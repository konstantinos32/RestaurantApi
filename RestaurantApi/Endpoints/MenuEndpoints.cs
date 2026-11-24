using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Models;

namespace RestaurantApi.Endpoints;

public static class MenuEndpoints
{
    public static RouteGroupBuilder MapMenuEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/menu")
            .WithTags("Menu");

        // Get all menu items, optionally filtering by name, category and price.
        group.MapGet("/", async (
            string? q,
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            RestaurantDbContext db) =>
        {
            var query = db.MenuItems
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(item =>
                    EF.Functions.Like(item.Name, $"%{q}%"));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(item =>
                    item.Category == category);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(item =>
                    item.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(item =>
                    item.Price <= maxPrice.Value);
            }

            var items = await query
                .Select(item => new MenuItemResponse(
                    item.Id,
                    item.Name,
                    item.Description,
                    item.Price,
                    item.Category,
                    item.IsAvailable
                ))
                .ToListAsync();

            return Results.Ok(items);
        })
        .WithName("GetMenuItems")
        .WithSummary("Get and filter menu items")
        .Produces<List<MenuItemResponse>>(StatusCodes.Status200OK);

        // Get one menu item by its ID.
        group.MapGet("/{id:int}", GetMenuItemById)
            .WithName("GetMenuItemById")
            .WithSummary("Get a menu item by ID")
            .Produces<MenuItemResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Create a new menu item.
        group.MapPost("/", async (
            CreateMenuItemRequest request,
            RestaurantDbContext db) =>
        {
            var item = new MenuItem
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                IsAvailable = true
            };

            db.MenuItems.Add(item);
            await db.SaveChangesAsync();

            var response = new MenuItemResponse(
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.Category,
                item.IsAvailable
            );

            return Results.Created($"/menu/{item.Id}", response);
        })
        .WithName("CreateMenuItem")
        .WithSummary("Create a menu item")
        .Produces<MenuItemResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        // Update an existing menu item.
        group.MapPut("/{id:int}", async (
            int id,
            UpdateMenuItemRequest request,
            RestaurantDbContext db) =>
        {
            var item = await db.MenuItems.FindAsync(id);

            if (item is null)
            {
                return Results.NotFound();
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.Category = request.Category;
            item.IsAvailable = request.IsAvailable;

            await db.SaveChangesAsync();

            var response = new MenuItemResponse(
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.Category,
                item.IsAvailable
            );

            return Results.Ok(response);
        })
        .WithName("UpdateMenuItem")
        .WithSummary("Update an existing menu item")
        .Produces<MenuItemResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        // Delete a menu item if it has not been used in an order.
        group.MapDelete("/{id:int}", async (
            int id,
            RestaurantDbContext db) =>
        {
            var item = await db.MenuItems.FindAsync(id);

            if (item is null)
            {
                return Results.NotFound();
            }

            // Keep historical order records intact by preventing deletion
            // of menu items that are already referenced by an order.
            var isUsedInOrder = await db.OrderItems
                .AnyAsync(orderItem => orderItem.MenuItemId == id);

            if (isUsedInOrder)
            {
                return Results.Conflict(new
                {
                    message = "This menu item cannot be deleted because it is used in an order."
                });
            }

            db.MenuItems.Remove(item);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteMenuItem")
        .WithSummary("Delete a menu item")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        return group;
    }

    private static async Task<IResult> GetMenuItemById(
        int id,
        RestaurantDbContext db)
    {
        var item = await db.MenuItems
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new MenuItemResponse(
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.Category,
                item.IsAvailable
            ))
            .FirstOrDefaultAsync();

        if (item is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(item);
    }
}