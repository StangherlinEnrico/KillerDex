namespace Application.DTOs;

public record StatusEffectDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    string Type,
    string AppliesTo,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record StatusEffectSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl,
    string Type
);
