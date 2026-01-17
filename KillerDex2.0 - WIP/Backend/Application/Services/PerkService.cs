using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
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
}
