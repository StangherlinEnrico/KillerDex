using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public record CreateChapterRequest(
    [Required][MaxLength(100)] string Name,
    [Required][Range(0, int.MaxValue)] int Number,
    DateOnly? ReleaseDate,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);

public record UpdateChapterRequest(
    [MaxLength(100)] string? Name,
    [Range(0, int.MaxValue)] int? Number,
    DateOnly? ReleaseDate,
    [MaxLength(500)] string? ImageUrl,
    [MaxLength(20)] string? GameVersion
);
