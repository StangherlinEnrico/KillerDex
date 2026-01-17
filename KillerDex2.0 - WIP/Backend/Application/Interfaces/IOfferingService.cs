using Application.DTOs;

namespace Application.Interfaces;

public interface IOfferingService
{
    Task<IEnumerable<OfferingSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OfferingSummaryDto>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<OfferingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OfferingDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
