namespace Application.DTOs;

public record ItemDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    string Type,
    string Rarity,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ItemSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl,
    string Type,
    string Rarity
);
