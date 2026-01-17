using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class SurvivorService : ISurvivorService
{
    private readonly DbdContext _context;
    private readonly IPerkRepository _perkRepository;

    public SurvivorService(DbdContext context, IPerkRepository perkRepository)
    {
        _context = context;
        _perkRepository = perkRepository;
    }

    public async Task<IEnumerable<SurvivorSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var survivors = await _context.Survivors
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return survivors.Select(s => s.ToSummaryDto());
    }

    public async Task<SurvivorDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var survivor = await _context.Survivors
            .Include(s => s.Chapter)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return survivor?.ToDto();
    }

    public async Task<SurvivorDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var survivor = await _context.Survivors
            .Include(s => s.Chapter)
            .FirstOrDefaultAsync(s => s.Slug == slug.ToLowerInvariant(), cancellationToken);

        return survivor?.ToDto();
    }

    public async Task<IEnumerable<PerkSummaryDto>> GetPerksAsync(Guid survivorId, CancellationToken cancellationToken = default)
    {
        var perks = await _perkRepository.GetBySurvivorIdAsync(survivorId, cancellationToken);
        return perks.Select(p => p.ToSummaryDto());
    }
}
