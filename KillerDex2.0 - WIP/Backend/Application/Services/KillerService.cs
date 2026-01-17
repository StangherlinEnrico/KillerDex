using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class KillerService : IKillerService
{
    private readonly DbdContext _context;
    private readonly IPerkRepository _perkRepository;
    private readonly IKillerAddonRepository _addonRepository;

    public KillerService(DbdContext context, IPerkRepository perkRepository, IKillerAddonRepository addonRepository)
    {
        _context = context;
        _perkRepository = perkRepository;
        _addonRepository = addonRepository;
    }

    public async Task<IEnumerable<KillerSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var killers = await _context.Killers
            .OrderBy(k => k.Name)
            .ToListAsync(cancellationToken);

        return killers.Select(k => k.ToSummaryDto());
    }

    public async Task<KillerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var killer = await _context.Killers
            .Include(k => k.Chapter)
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);

        return killer?.ToDto();
    }

    public async Task<KillerDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var killer = await _context.Killers
            .Include(k => k.Chapter)
            .FirstOrDefaultAsync(k => k.Slug == slug.ToLowerInvariant(), cancellationToken);

        return killer?.ToDto();
    }

    public async Task<IEnumerable<PerkSummaryDto>> GetPerksAsync(Guid killerId, CancellationToken cancellationToken = default)
    {
        var perks = await _perkRepository.GetByKillerIdAsync(killerId, cancellationToken);
        return perks.Select(p => p.ToSummaryDto());
    }

    public async Task<IEnumerable<AddonSummaryDto>> GetAddonsAsync(Guid killerId, CancellationToken cancellationToken = default)
    {
        var addons = await _addonRepository.GetByKillerIdAsync(killerId, cancellationToken);
        return addons.Select(a => a.ToSummaryDto());
    }
}
