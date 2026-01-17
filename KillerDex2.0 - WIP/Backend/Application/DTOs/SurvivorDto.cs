namespace Application.DTOs;

public record SurvivorDto(
    Guid Id,
    string Slug,
    string Name,
    string? Overview,
    string? Backstory,
    string? ImageUrl,
    string? GameVersion,
    ChapterSummaryDto? Chapter,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record SurvivorSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl
);
