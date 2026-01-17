using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateRealmRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    Guid? KillerId,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateRealmRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    Guid? KillerId,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record CreateMapRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] Guid RealmId,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateMapRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);
