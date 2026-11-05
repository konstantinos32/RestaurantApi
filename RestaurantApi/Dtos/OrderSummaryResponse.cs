namespace RestaurantApi.Dtos;

public record OrderSummaryResponse(
    int Id,
    DateTimeOffset CreatedAt,
    int ItemCount,
    decimal TotalAmount
);