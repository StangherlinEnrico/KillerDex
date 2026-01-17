using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreatePerkRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(4000)] string? Description,
    [Required] string Role,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion,
    Guid? KillerId,
    Guid? SurvivorId
);

public record UpdatePerkRequest(
    [MaxLength(200)] string? Name,
    [MaxLength(4000)] string? Description,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion,
    Guid? KillerId,
    Guid? SurvivorId
);
