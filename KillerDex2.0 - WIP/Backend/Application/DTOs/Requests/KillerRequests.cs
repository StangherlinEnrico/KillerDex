using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateKillerRequest(
    [Required][MaxLength(100)] string Name,
    [MaxLength(100)] string? RealName,
    [MaxLength(2000)] string? Overview,
    [MaxLength(4000)] string? Backstory,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion,
    [Required] PowerRequest Power,
    [Required][Range(0.01, 10.0)] decimal MovementSpeed,
    [Required][Range(0, 100)] int TerrorRadius,
    [Required] string Height,
    Guid? ChapterId
);

public record UpdateKillerRequest(
    [MaxLength(100)] string? Name,
    [MaxLength(100)] string? RealName,
    [MaxLength(2000)] string? Overview,
    [MaxLength(4000)] string? Backstory,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion,
    PowerRequest? Power,
    [Range(0.01, 10.0)] decimal? MovementSpeed,
    [Range(0, 100)] int? TerrorRadius,
    string? Height,
    Guid? ChapterId
);

public record PowerRequest(
    [Required][MaxLength(100)] string Name,
    [MaxLength(2000)] string? Description
);
