using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PerkService : IPerkService
{
    private readonly DbdContext _context;

    public PerkService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PerkSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var perks = await _context.Perks
            .OrderBy(p => p.Role)
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return perks.Select(p => p.ToSummaryDto());
    }

    public async Task<IEnumerable<PerkSummaryDto>> GetByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Role>(role, true, out var roleEnum))
            return Enumerable.Empty<PerkSummaryDto>();

        var perks = await _context.Perks
            .Where(p => p.Role == roleEnum)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return perks.Select(p => p.ToSummaryDto());
    }

    public async Task<PerkDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var perk = await _context.Perks
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return perk?.ToDto();
    }

    public async Task<PerkDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var perk = await _context.Perks
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .FirstOrDefaultAsync(p => p.Slug == slug.ToLowerInvariant(), cancellationToken);

        return perk?.ToDto();
    }

    public async Task<PerkDto> CreateAsync(CreatePerkRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Role>(request.Role, true, out var role))
            throw new ArgumentException($"Invalid role: {request.Role}");

        var perk = new Perk(
            name: request.Name,
            role: role,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            perk.SetImageUrl(request.ImageUrl);

        if (request.KillerId.HasValue)
            perk.AssignToKiller(request.KillerId.Value);
        else if (request.SurvivorId.HasValue)
            perk.AssignToSurvivor(request.SurvivorId.Value);

        _context.Perks.Add(perk);
        await _context.SaveChangesAsync(cancellationToken);

        return perk.ToDto();
    }

    public async Task<PerkDto?> UpdateAsync(Guid id, UpdatePerkRequest request, CancellationToken cancellationToken = default)
    {
        var perk = await _context.Perks
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (perk is null)
            return null;

        perk.Update(
            name: request.Name,
            description: request.Description
        );

        if (request.ImageUrl is not null)
            perk.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            perk.SetGameVersion(request.GameVersion);

        if (request.KillerId.HasValue)
            perk.AssignToKiller(request.KillerId.Value);
        else if (request.SurvivorId.HasValue)
            perk.AssignToSurvivor(request.SurvivorId.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return perk.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var perk = await _context.Perks.FindAsync([id], cancellationToken);

        if (perk is null)
            return false;

        _context.Perks.Remove(perk);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
