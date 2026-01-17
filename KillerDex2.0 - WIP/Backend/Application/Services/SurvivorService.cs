using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
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

    public async Task<SurvivorDto> CreateAsync(CreateSurvivorRequest request, CancellationToken cancellationToken = default)
    {
        var survivor = new Survivor(
            name: request.Name,
            overview: request.Overview,
            backstory: request.Backstory,
            chapterId: request.ChapterId,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            survivor.SetImageUrl(request.ImageUrl);

        _context.Survivors.Add(survivor);
        await _context.SaveChangesAsync(cancellationToken);

        return survivor.ToDto();
    }

    public async Task<SurvivorDto?> UpdateAsync(Guid id, UpdateSurvivorRequest request, CancellationToken cancellationToken = default)
    {
        var survivor = await _context.Survivors
            .Include(s => s.Chapter)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (survivor is null)
            return null;

        survivor.Update(
            name: request.Name,
            overview: request.Overview,
            backstory: request.Backstory
        );

        if (request.ImageUrl is not null)
            survivor.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            survivor.SetGameVersion(request.GameVersion);

        if (request.ChapterId.HasValue)
            survivor.AssignToChapter(request.ChapterId.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return survivor.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var survivor = await _context.Survivors.FindAsync([id], cancellationToken);

        if (survivor is null)
            return false;

        _context.Survivors.Remove(survivor);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
