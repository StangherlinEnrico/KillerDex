using Application.DTOs;

namespace Application.Interfaces;

public interface IPerkService
{
    Task<IEnumerable<PerkSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PerkSummaryDto>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<PerkDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PerkDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
