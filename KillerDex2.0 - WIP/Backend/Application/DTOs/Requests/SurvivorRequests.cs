using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateSurvivorRequest(
    [Required][MaxLength(100)] string Name,
    [MaxLength(2000)] string? Overview,
    [MaxLength(8000)] string? Backstory,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion,
    Guid? ChapterId
);

public record UpdateSurvivorRequest(
    [MaxLength(100)] string? Name,
    [MaxLength(2000)] string? Overview,
    [MaxLength(8000)] string? Backstory,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion,
    Guid? ChapterId
);
