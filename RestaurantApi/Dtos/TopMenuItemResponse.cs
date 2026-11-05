namespace RestaurantApi.Dtos;

public record TopMenuItemResponse(
    int MenuItemId,
    string MenuItemName,
    int QuantitySold
);