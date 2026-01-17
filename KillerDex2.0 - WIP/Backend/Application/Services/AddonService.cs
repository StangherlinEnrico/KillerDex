using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
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
}
