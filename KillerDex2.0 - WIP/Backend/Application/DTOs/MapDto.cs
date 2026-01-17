namespace Application.DTOs;

public record RealmDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    KillerSummaryDto? Killer,
    IEnumerable<MapSummaryDto> Maps,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record RealmSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl
);

public record MapDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? GameVersion,
    RealmSummaryDto Realm,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record MapSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    string? ImageUrl
);
