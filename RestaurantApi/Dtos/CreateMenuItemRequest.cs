using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public record CreateMenuItemRequest(
    [Required]
    [StringLength(100, MinimumLength = 2)]
    string Name,

    [Required]
    [StringLength(500)]
    string Description,

    [Range(0.01, 999999.99)]
    decimal Price,

    [Required]
    [StringLength(50, MinimumLength = 2)]
    string Category
);