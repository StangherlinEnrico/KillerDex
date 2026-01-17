using Application.DTOs;

namespace Application.Interfaces;

public interface IKillerAddonService
{
    Task<IEnumerable<AddonSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default);
    Task<KillerAddonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<KillerAddonDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}

public interface ISurvivorAddonService
{
    Task<IEnumerable<AddonSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetByItemTypeAsync(string itemType, CancellationToken cancellationToken = default);
    Task<SurvivorAddonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SurvivorAddonDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
