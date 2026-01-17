using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateKillerAddonRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] string Rarity,
    [Required] Guid KillerId,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateKillerAddonRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    string? Rarity,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record CreateSurvivorAddonRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] string Rarity,
    [Required] string ItemType,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateSurvivorAddonRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    string? Rarity,
    string? ItemType,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);
