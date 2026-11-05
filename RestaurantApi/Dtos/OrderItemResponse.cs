namespace RestaurantApi.Dtos;

public record OrderItemResponse(
    int MenuItemId,
    string MenuItemName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);