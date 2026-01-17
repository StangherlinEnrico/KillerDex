using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateOfferingRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] string Rarity,
    [Required] string Role,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateOfferingRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    string? Rarity,
    string? Role,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);
