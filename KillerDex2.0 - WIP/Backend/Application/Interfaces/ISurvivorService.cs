using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface ISurvivorService
{
    Task<IEnumerable<SurvivorSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SurvivorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SurvivorDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<PerkSummaryDto>> GetPerksAsync(Guid survivorId, CancellationToken cancellationToken = default);

    Task<SurvivorDto> CreateAsync(CreateSurvivorRequest request, CancellationToken cancellationToken = default);
    Task<SurvivorDto?> UpdateAsync(Guid id, UpdateSurvivorRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
