using Application.DTOs;

namespace Application.Interfaces;

public interface ISurvivorService
{
    Task<IEnumerable<SurvivorSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SurvivorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SurvivorDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<PerkSummaryDto>> GetPerksAsync(Guid survivorId, CancellationToken cancellationToken = default);
}
