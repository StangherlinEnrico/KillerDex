using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IKillerAddonService
{
    Task<IEnumerable<AddonSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default);
    Task<KillerAddonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<KillerAddonDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<KillerAddonDto> CreateAsync(CreateKillerAddonRequest request, CancellationToken cancellationToken = default);
    Task<KillerAddonDto?> UpdateAsync(Guid id, UpdateKillerAddonRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ISurvivorAddonService
{
    Task<IEnumerable<AddonSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetByItemTypeAsync(string itemType, CancellationToken cancellationToken = default);
    Task<SurvivorAddonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SurvivorAddonDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<SurvivorAddonDto> CreateAsync(CreateSurvivorAddonRequest request, CancellationToken cancellationToken = default);
    Task<SurvivorAddonDto?> UpdateAsync(Guid id, UpdateSurvivorAddonRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
