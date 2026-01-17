using Application.DTOs;

namespace Application.Interfaces;

public interface IRealmService
{
    Task<IEnumerable<RealmSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RealmDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RealmDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}

public interface IMapService
{
    Task<IEnumerable<MapSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MapSummaryDto>> GetByRealmIdAsync(Guid realmId, CancellationToken cancellationToken = default);
    Task<MapDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MapDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
