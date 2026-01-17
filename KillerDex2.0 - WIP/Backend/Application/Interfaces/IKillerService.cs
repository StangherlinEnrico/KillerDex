using Application.DTOs;

namespace Application.Interfaces;

public interface IKillerService
{
    Task<IEnumerable<KillerSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<KillerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<KillerDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<PerkSummaryDto>> GetPerksAsync(Guid killerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetAddonsAsync(Guid killerId, CancellationToken cancellationToken = default);
}
