using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateItemRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] string Type,
    [Required] string Rarity,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateItemRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    string? Rarity,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);
