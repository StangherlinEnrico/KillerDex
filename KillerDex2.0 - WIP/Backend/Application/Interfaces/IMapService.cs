using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IRealmService
{
    Task<IEnumerable<RealmSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RealmDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RealmDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<RealmDto> CreateAsync(CreateRealmRequest request, CancellationToken cancellationToken = default);
    Task<RealmDto?> UpdateAsync(Guid id, UpdateRealmRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IMapService
{
    Task<IEnumerable<MapSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MapSummaryDto>> GetByRealmIdAsync(Guid realmId, CancellationToken cancellationToken = default);
    Task<MapDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MapDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<MapDto> CreateAsync(CreateMapRequest request, CancellationToken cancellationToken = default);
    Task<MapDto?> UpdateAsync(Guid id, UpdateMapRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
