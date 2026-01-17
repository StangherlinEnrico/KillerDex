using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IKillerService
{
    // Read operations
    Task<IEnumerable<KillerSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<KillerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<KillerDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<PerkSummaryDto>> GetPerksAsync(Guid killerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetAddonsAsync(Guid killerId, CancellationToken cancellationToken = default);

    // Write operations
    Task<KillerDto> CreateAsync(CreateKillerRequest request, CancellationToken cancellationToken = default);
    Task<KillerDto?> UpdateAsync(Guid id, UpdateKillerRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
