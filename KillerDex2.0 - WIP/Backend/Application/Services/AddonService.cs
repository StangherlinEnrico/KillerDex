using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class KillerAddonService : IKillerAddonService
{
    private readonly DbdContext _context;

    public KillerAddonService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AddonSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var addons = await _context.KillerAddons
            .Include(a => a.Killer)
            .OrderBy(a => a.Killer.Name)
            .ThenBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return addons.Select(a => a.ToSummaryDto());
    }

    public async Task<IEnumerable<AddonSummaryDto>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default)
    {
        var addons = await _context.KillerAddons
            .Where(a => a.KillerId == killerId)
            .OrderBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return addons.Select(a => a.ToSummaryDto());
    }

    public async Task<KillerAddonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var addon = await _context.KillerAddons
            .Include(a => a.Killer)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return addon?.ToDto();
    }

    public async Task<KillerAddonDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var addon = await _context.KillerAddons
            .Include(a => a.Killer)
            .FirstOrDefaultAsync(a => a.Slug == slug.ToLowerInvariant(), cancellationToken);

        return addon?.ToDto();
    }

    public async Task<KillerAddonDto> CreateAsync(CreateKillerAddonRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Rarity>(request.Rarity, true, out var rarity))
            throw new ArgumentException($"Invalid rarity: {request.Rarity}");

        var addon = new KillerAddon(
            name: request.Name,
            rarity: rarity,
            killerId: request.KillerId,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            addon.SetImageUrl(request.ImageUrl);

        _context.KillerAddons.Add(addon);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(addon).Reference(a => a.Killer).LoadAsync(cancellationToken);
        return addon.ToDto();
    }

    public async Task<KillerAddonDto?> UpdateAsync(Guid id, UpdateKillerAddonRequest request, CancellationToken cancellationToken = default)
    {
        var addon = await _context.KillerAddons
            .Include(a => a.Killer)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (addon is null)
            return null;

        Rarity? rarity = null;
        if (request.Rarity is not null && Enum.TryParse<Rarity>(request.Rarity, true, out var parsedRarity))
            rarity = parsedRarity;

        addon.Update(name: request.Name, description: request.Description, rarity: rarity);

        if (request.ImageUrl is not null)
            addon.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            addon.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);
        return addon.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var addon = await _context.KillerAddons.FindAsync([id], cancellationToken);
        if (addon is null) return false;

        _context.KillerAddons.Remove(addon);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class SurvivorAddonService : ISurvivorAddonService
{
    private readonly DbdContext _context;

    public SurvivorAddonService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AddonSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var addons = await _context.SurvivorAddons
            .OrderBy(a => a.ItemType)
            .ThenBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return addons.Select(a => a.ToSummaryDto());
    }

    public async Task<IEnumerable<AddonSummaryDto>> GetByItemTypeAsync(string itemType, CancellationToken cancellationToken = default)
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

    public async Task<SurvivorAddonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var addon = await _context.SurvivorAddons
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return addon?.ToDto();
    }

    public async Task<SurvivorAddonDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var addon = await _context.SurvivorAddons
            .FirstOrDefaultAsync(a => a.Slug == slug.ToLowerInvariant(), cancellationToken);

        return addon?.ToDto();
    }

    public async Task<SurvivorAddonDto> CreateAsync(CreateSurvivorAddonRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Rarity>(request.Rarity, true, out var rarity))
            throw new ArgumentException($"Invalid rarity: {request.Rarity}");

        if (!Enum.TryParse<ItemType>(request.ItemType, true, out var itemType))
            throw new ArgumentException($"Invalid item type: {request.ItemType}");

        var addon = new SurvivorAddon(
            name: request.Name,
            rarity: rarity,
            itemType: itemType,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            addon.SetImageUrl(request.ImageUrl);

        _context.SurvivorAddons.Add(addon);
        await _context.SaveChangesAsync(cancellationToken);

        return addon.ToDto();
    }

    public async Task<SurvivorAddonDto?> UpdateAsync(Guid id, UpdateSurvivorAddonRequest request, CancellationToken cancellationToken = default)
    {
        var addon = await _context.SurvivorAddons.FindAsync([id], cancellationToken);
        if (addon is null) return null;

        Rarity? rarity = null;
        if (request.Rarity is not null && Enum.TryParse<Rarity>(request.Rarity, true, out var parsedRarity))
            rarity = parsedRarity;

        ItemType? itemType = null;
        if (request.ItemType is not null && Enum.TryParse<ItemType>(request.ItemType, true, out var parsedItemType))
            itemType = parsedItemType;

        addon.Update(name: request.Name, description: request.Description, rarity: rarity, itemType: itemType);

        if (request.ImageUrl is not null)
            addon.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            addon.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);
        return addon.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var addon = await _context.SurvivorAddons.FindAsync([id], cancellationToken);
        if (addon is null) return false;

        _context.SurvivorAddons.Remove(addon);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
