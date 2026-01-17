namespace Application.DTOs;

public record PerkDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    string Role,
    OwnerSummaryDto? Owner,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record PerkSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl,
    string Role
);

public record OwnerSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string Type
);
