using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;

using Microsoft.AspNetCore.Routing;

namespace RestaurantApi.Endpoints;

public static class ReportEndpoints
{
    public static RouteGroupBuilder MapReportEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/reports")
            .WithTags("Reports");

        group.MapGet("/daily", async (DateOnly date, RestaurantDbContext db) =>
{
var start = new DateTimeOffset(
    date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
);

var end = start.AddDays(1);

var orderCount = await db.Orders
    .Where(order => order.CreatedAt >= start &&
                    order.CreatedAt < end)
    .CountAsync();

var totalRevenue = await db.Orders
.Where(order => order.CreatedAt >= start &&
                order.CreatedAt < end)
.SumAsync(order => (decimal?)order.TotalAmount) ?? 0m;



var itemTotals = db.OrderItems
    .Join(
        db.Orders,
        item => item.OrderId,
        order => order.Id,
        (item, order) => new { item, order }
    )
    .Where(x => x.order.CreatedAt >= start &&
                x.order.CreatedAt < end)
    .GroupBy(x => x.item.MenuItemId)
    .Select(group => new
    {
        MenuItemId = group.Key,
        QuantitySold = group.Sum(x => x.item.Quantity)
    });

var topItems = await itemTotals
    .Join(
        db.MenuItems,
        total => total.MenuItemId,
        menuItem => menuItem.Id,
        (total, menuItem) => new
        {
            total.MenuItemId,
            MenuItemName = menuItem.Name,
            total.QuantitySold
        }
    )
    .OrderByDescending(item => item.QuantitySold)
    .ThenBy(item => item.MenuItemName)
    .Take(3)
    .Select(item => new TopMenuItemResponse(
        item.MenuItemId,
        item.MenuItemName,
        item.QuantitySold
    ))
    .ToListAsync();



return Results.Ok(new DailyReportResponse(
    date,
    orderCount,
    totalRevenue,
    topItems
));
})
.WithName("GetDailyReport")
.WithSummary("Get a daily report of restaurant orders")
.Produces<DailyReportResponse>(StatusCodes.Status200OK);

        return group;
    }
}