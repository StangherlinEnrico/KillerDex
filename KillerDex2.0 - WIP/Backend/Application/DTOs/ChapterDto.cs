namespace Application.DTOs;

public record ChapterDto(
    Guid Id,
    string Slug,
    string Name,
    int Number,
    DateOnly? ReleaseDate,
    string? ImageUrl,
    string? GameVersion,
    IEnumerable<KillerSummaryDto> Killers,
    IEnumerable<SurvivorSummaryDto> Survivors,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ChapterSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    int Number
);
