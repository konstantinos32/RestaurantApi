using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public record CreateOrderItemRequest(
    [Range(1, int.MaxValue)]
    int MenuItemId,

    [Range(1, 100)]
    int Quantity
);