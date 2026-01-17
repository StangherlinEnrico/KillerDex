using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IPerkService
{
    Task<IEnumerable<PerkSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PerkSummaryDto>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<PerkDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PerkDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<PerkDto> CreateAsync(CreatePerkRequest request, CancellationToken cancellationToken = default);
    Task<PerkDto?> UpdateAsync(Guid id, UpdatePerkRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
