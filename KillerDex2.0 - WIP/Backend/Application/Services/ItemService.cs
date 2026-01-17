using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
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

    public async Task<ItemDto> CreateAsync(CreateItemRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ItemType>(request.Type, true, out var itemType))
            throw new ArgumentException($"Invalid item type: {request.Type}");

        if (!Enum.TryParse<Rarity>(request.Rarity, true, out var rarity))
            throw new ArgumentException($"Invalid rarity: {request.Rarity}");

        var item = new Item(
            name: request.Name,
            type: itemType,
            rarity: rarity,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            item.SetImageUrl(request.ImageUrl);

        _context.Items.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return item.ToDto();
    }

    public async Task<ItemDto?> UpdateAsync(Guid id, UpdateItemRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _context.Items.FindAsync([id], cancellationToken);
        if (item is null) return null;

        Rarity? rarity = null;
        if (request.Rarity is not null && Enum.TryParse<Rarity>(request.Rarity, true, out var parsedRarity))
            rarity = parsedRarity;

        item.Update(name: request.Name, description: request.Description, rarity: rarity);

        if (request.ImageUrl is not null)
            item.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            item.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);
        return item.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _context.Items.FindAsync([id], cancellationToken);
        if (item is null) return false;

        _context.Items.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
