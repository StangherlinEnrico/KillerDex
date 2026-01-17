using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IStatusEffectService
{
    Task<IEnumerable<StatusEffectSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StatusEffectSummaryDto>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<StatusEffectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StatusEffectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<StatusEffectDto> CreateAsync(CreateStatusEffectRequest request, CancellationToken cancellationToken = default);
    Task<StatusEffectDto?> UpdateAsync(Guid id, UpdateStatusEffectRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
