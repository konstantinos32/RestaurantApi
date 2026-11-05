using RestaurantApi.Models;

namespace RestaurantApi.Dtos;

public record OrderResponse(
    int Id,
    DateTimeOffset CreatedAt,
    OrderStatus Status,
    List<OrderItemResponse> Items,
    decimal TotalAmount
);
