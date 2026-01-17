namespace Application.DTOs;

public record OfferingDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    string Rarity,
    string Role,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record OfferingSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl,
    string Rarity,
    string Role
);
