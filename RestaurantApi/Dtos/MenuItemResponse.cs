namespace RestaurantApi.Dtos;

public record MenuItemResponse (
    int Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    bool IsAvailable
);