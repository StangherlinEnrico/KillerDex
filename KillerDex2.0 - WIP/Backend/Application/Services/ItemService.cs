using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ItemService : IItemService
{
    private readonly DbdContext _context;

    public ItemService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItemSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _context.Items
            .OrderBy(i => i.Type)
            .ThenBy(i => i.Rarity)
            .ThenBy(i => i.Name)
            .ToListAsync(cancellationToken);

        return items.Select(i => i.ToSummaryDto());
    }

    public async Task<IEnumerable<ItemSummaryDto>> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ItemType>(type, true, out var itemTypeEnum))
            return Enumerable.Empty<ItemSummaryDto>();

        var items = await _context.Items
            .Where(i => i.Type == itemTypeEnum)
            .OrderBy(i => i.Rarity)
            .ThenBy(i => i.Name)
            .ToListAsync(cancellationToken);

        return items.Select(i => i.ToSummaryDto());
    }

    public async Task<ItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        return item?.ToDto();
    }

    public async Task<ItemDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Slug == slug.ToLowerInvariant(), cancellationToken);

        return item?.ToDto();
    }

    public async Task<IEnumerable<AddonSummaryDto>> GetAddonsAsync(string itemType, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ItemType>(itemType, true, out var itemTypeEnum))
            return Enumerable.Empty<AddonSummaryDto>();

        var addons = await _context.SurvivorAddons
            .Where(a => a.ItemType == itemTypeEnum)
            .OrderBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return addons.Select(a => a.ToSummaryDto());
    }
}
