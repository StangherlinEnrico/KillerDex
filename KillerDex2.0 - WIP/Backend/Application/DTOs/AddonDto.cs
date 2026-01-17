namespace Application.DTOs;

public record KillerAddonDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    string Rarity,
    KillerSummaryDto Killer,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record SurvivorAddonDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    string Rarity,
    string ItemType,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record AddonSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl,
    string Rarity
);
