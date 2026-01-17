using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class RealmService : IRealmService
{
    private readonly DbdContext _context;

    public RealmService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RealmSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var realms = await _context.Realms
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        return realms.Select(r => r.ToSummaryDto());
    }

    public async Task<RealmDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var realm = await _context.Realms
            .Include(r => r.Killer)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (realm is null) return null;

        var maps = await _context.Maps
            .Where(m => m.RealmId == realm.Id)
            .ToListAsync(cancellationToken);

        return realm.ToDto(maps);
    }

    public async Task<RealmDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var realm = await _context.Realms
            .Include(r => r.Killer)
            .FirstOrDefaultAsync(r => r.Slug == slug.ToLowerInvariant(), cancellationToken);

        if (realm is null) return null;

        var maps = await _context.Maps
            .Where(m => m.RealmId == realm.Id)
            .ToListAsync(cancellationToken);

        return realm.ToDto(maps);
    }
}

public class MapService : IMapService
{
    private readonly DbdContext _context;

    public MapService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MapSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var maps = await _context.Maps
            .Include(m => m.Realm)
            .OrderBy(m => m.Realm.Name)
            .ThenBy(m => m.Name)
            .ToListAsync(cancellationToken);

        return maps.Select(m => m.ToSummaryDto());
    }

    public async Task<IEnumerable<MapSummaryDto>> GetByRealmIdAsync(Guid realmId, CancellationToken cancellationToken = default)
    {
        var maps = await _context.Maps
            .Where(m => m.RealmId == realmId)
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);

        return maps.Select(m => m.ToSummaryDto());
    }

    public async Task<MapDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var map = await _context.Maps
            .Include(m => m.Realm)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        return map?.ToDto();
    }

    public async Task<MapDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var map = await _context.Maps
            .Include(m => m.Realm)
            .FirstOrDefaultAsync(m => m.Slug == slug.ToLowerInvariant(), cancellationToken);

        return map?.ToDto();
    }
}
