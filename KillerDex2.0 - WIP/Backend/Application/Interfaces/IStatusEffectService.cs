using Application.DTOs;

namespace Application.Interfaces;

public interface IStatusEffectService
{
    Task<IEnumerable<StatusEffectSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StatusEffectSummaryDto>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<StatusEffectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StatusEffectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
