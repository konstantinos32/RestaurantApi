using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public record CreateOrderRequest(
    [Required]
    [MinLength(1)]
    List<CreateOrderItemRequest> Items
);