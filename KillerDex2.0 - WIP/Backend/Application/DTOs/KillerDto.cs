namespace Application.DTOs;

public record KillerDto(
    Guid Id,
    string Slug,
    string Name,
    string? RealName,
    string? Overview,
    string? Backstory,
    string? ImageUrl,
    string? GameVersion,
    PowerDto Power,
    decimal MovementSpeed,
    int TerrorRadius,
    string Height,
    ChapterSummaryDto? Chapter,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record KillerSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl,
    string PowerName
);

public record PowerDto(
    string Name,
    string? Description
);
