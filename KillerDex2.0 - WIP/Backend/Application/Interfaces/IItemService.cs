using Application.DTOs;

namespace Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ItemSummaryDto>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<ItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ItemDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetAddonsAsync(string itemType, CancellationToken cancellationToken = default);
}
