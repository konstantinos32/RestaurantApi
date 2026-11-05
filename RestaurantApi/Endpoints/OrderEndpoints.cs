
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Models;

namespace RestaurantApi.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrderEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/orders")
            .WithTags("Orders");

        group.MapGet("/", async (RestaurantDbContext db) =>
{
    var orders = await db.Orders
        .Select(order => new OrderSummaryResponse(
            order.Id,
            order.CreatedAt,
            order.Items.Sum(item => item.Quantity),
            order.TotalAmount
        ))
        .ToListAsync();

    return Results.Ok(orders);
})

.WithName("GetOrders")
.WithSummary("Get a summary of all orders")
.Produces<List<OrderSummaryResponse>>(StatusCodes.Status200OK);
        group.MapGet("/{id:int}", async (int id, RestaurantDbContext db) =>
        {
            var order = await db.Orders
                .Where(order => order.Id == id)
                .Select(order => new OrderResponse(
                    order.Id,
                    order.CreatedAt,
                    order.Status,
                    order.Items.Select(item => new OrderItemResponse(
                        item.MenuItemId,
                        item.MenuItem!.Name,
                        item.Quantity,
                        item.UnitPrice,
                        item.UnitPrice * item.Quantity
                    )).ToList(),
                    order.TotalAmount
                ))
                .FirstOrDefaultAsync();

            if (order is null)
            {
                return Results.NotFound(new
                {
                    message = $"Order with ID {id} was not found."
                });
            }

            return Results.Ok(order);
        })
        .WithName("GetOrderById")
        .WithSummary("Get a specific order with its items")
        .Produces<OrderResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
            CreateOrderRequest request,
            RestaurantDbContext db) =>
        {
            // Reject duplicate menu item IDs in the same order.
            var hasDuplicateItems = request.Items
                .GroupBy(item => item.MenuItemId)
                .Any(group => group.Count() > 1);

            if (hasDuplicateItems)
            {
                return Results.BadRequest(new
                {
                    message = "The same menu item cannot appear more than once."
                });
            }

            // Extract the requested menu item IDs.
            var menuItemIds = request.Items
                .Select(item => item.MenuItemId)
                .ToList();

            // Retrieve the matching menu items from the database.
            var menuItems = await db.MenuItems
                .Where(item => menuItemIds.Contains(item.Id))
                .ToListAsync();

            // Check whether all requested menu items exist.
            if (menuItems.Count != menuItemIds.Count)
            {
                return Results.BadRequest(new
                {
                    message = "One or more menu items do not exist."
                });
            }

            // Check whether all requested menu items are available.
            var unavailableItems = menuItems
                .Where(item => !item.IsAvailable)
                .ToList();

            if (unavailableItems.Count > 0)
            {
                return Results.BadRequest(new
                {
                    message = "One or more menu items are currently unavailable."
                });
            }
            // Create the order with its initial values.
            var order = new Order
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Status = OrderStatus.Pending
            };

            // Add each requested menu item to the order.
            foreach (var requestedItem in request.Items)
            {
                var menuItem = menuItems.First(
                    item => item.Id == requestedItem.MenuItemId);

                var orderItem = new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = requestedItem.Quantity,
                    UnitPrice = menuItem.Price
                };

                order.Items.Add(orderItem);
            }

            // Calculate the total using the prices stored in the order.
            order.TotalAmount = order.Items.Sum(
                item => item.UnitPrice * item.Quantity);

            // Add the order and its items to the database.
            db.Orders.Add(order);

            // Save the order and all its items in one database operation.
            await db.SaveChangesAsync();

            // Build the response using the saved order and menu item details.
            var response = new OrderResponse(
                order.Id,
                order.CreatedAt,
                order.Status,
                order.Items.Select(orderItem =>
                {
                    var menuItem = menuItems.First(
                        item => item.Id == orderItem.MenuItemId);

                    return new OrderItemResponse(
                        menuItem.Id,
                        menuItem.Name,
                        orderItem.Quantity,
                        orderItem.UnitPrice,
                        orderItem.UnitPrice * orderItem.Quantity
                    );
                }).ToList(),
                order.TotalAmount
            );

            // Return 201 Created with the new order's location and response.
            return Results.Created($"/orders/{order.Id}", response);

        })
        .WithName("CreateOrder")
        .WithSummary("Create a new restaurant order")
.Produces<OrderResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}
