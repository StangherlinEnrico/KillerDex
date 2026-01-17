using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
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

    public async Task<RealmDto> CreateAsync(CreateRealmRequest request, CancellationToken cancellationToken = default)
    {
        var realm = new Realm(
            name: request.Name,
            description: request.Description,
            killerId: request.KillerId,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            realm.SetImageUrl(request.ImageUrl);

        _context.Realms.Add(realm);
        await _context.SaveChangesAsync(cancellationToken);

        return realm.ToDto([]);
    }

    public async Task<RealmDto?> UpdateAsync(Guid id, UpdateRealmRequest request, CancellationToken cancellationToken = default)
    {
        var realm = await _context.Realms
            .Include(r => r.Killer)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (realm is null) return null;

        realm.Update(name: request.Name, description: request.Description);

        if (request.KillerId.HasValue)
            realm.AssignToKiller(request.KillerId.Value);

        if (request.ImageUrl is not null)
            realm.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            realm.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);

        var maps = await _context.Maps
            .Where(m => m.RealmId == realm.Id)
            .ToListAsync(cancellationToken);

        return realm.ToDto(maps);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var realm = await _context.Realms.FindAsync([id], cancellationToken);
        if (realm is null) return false;

        _context.Realms.Remove(realm);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
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

    public async Task<MapDto> CreateAsync(CreateMapRequest request, CancellationToken cancellationToken = default)
    {
        var map = new Map(
            name: request.Name,
            realmId: request.RealmId,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            map.SetImageUrl(request.ImageUrl);

        _context.Maps.Add(map);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(map).Reference(m => m.Realm).LoadAsync(cancellationToken);
        return map.ToDto();
    }

    public async Task<MapDto?> UpdateAsync(Guid id, UpdateMapRequest request, CancellationToken cancellationToken = default)
    {
        var map = await _context.Maps
            .Include(m => m.Realm)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (map is null) return null;

        map.Update(name: request.Name, description: request.Description);

        if (request.ImageUrl is not null)
            map.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            map.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);
        return map.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var map = await _context.Maps.FindAsync([id], cancellationToken);
        if (map is null) return false;

        _context.Maps.Remove(map);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
