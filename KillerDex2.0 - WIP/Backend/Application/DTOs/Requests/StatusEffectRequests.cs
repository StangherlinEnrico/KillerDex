using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateStatusEffectRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] string Type,
    [Required] string AppliesTo,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateStatusEffectRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    string? Type,
    string? AppliesTo,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);
