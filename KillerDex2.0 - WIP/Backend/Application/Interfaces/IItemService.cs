using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ItemSummaryDto>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<ItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ItemDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<AddonSummaryDto>> GetAddonsAsync(string itemType, CancellationToken cancellationToken = default);

    Task<ItemDto> CreateAsync(CreateItemRequest request, CancellationToken cancellationToken = default);
    Task<ItemDto?> UpdateAsync(Guid id, UpdateItemRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
