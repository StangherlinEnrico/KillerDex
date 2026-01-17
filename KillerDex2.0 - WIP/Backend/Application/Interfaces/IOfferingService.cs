using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IOfferingService
{
    Task<IEnumerable<OfferingSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OfferingSummaryDto>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<OfferingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OfferingDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<OfferingDto> CreateAsync(CreateOfferingRequest request, CancellationToken cancellationToken = default);
    Task<OfferingDto?> UpdateAsync(Guid id, UpdateOfferingRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
