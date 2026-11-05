namespace RestaurantApi.Dtos;

public record DailyReportResponse(
    DateOnly Date,
    int OrderCount,
    decimal TotalRevenue,
    List<TopMenuItemResponse> TopItems
);