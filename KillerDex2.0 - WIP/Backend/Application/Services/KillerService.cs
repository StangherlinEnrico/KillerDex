using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
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

    public async Task<KillerDto> CreateAsync(CreateKillerRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<KillerHeight>(request.Height, true, out var height))
            throw new ArgumentException($"Invalid height value: {request.Height}");

        var power = new Power(request.Power.Name, request.Power.Description);

        var killer = new Killer(
            name: request.Name,
            power: power,
            movementSpeed: request.MovementSpeed,
            terrorRadius: request.TerrorRadius,
            height: height,
            realName: request.RealName,
            overview: request.Overview,
            backstory: request.Backstory,
            chapterId: request.ChapterId,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            killer.SetImageUrl(request.ImageUrl);

        _context.Killers.Add(killer);
        await _context.SaveChangesAsync(cancellationToken);

        return killer.ToDto();
    }

    public async Task<KillerDto?> UpdateAsync(Guid id, UpdateKillerRequest request, CancellationToken cancellationToken = default)
    {
        var killer = await _context.Killers
            .Include(k => k.Chapter)
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);

        if (killer is null)
            return null;

        KillerHeight? height = null;
        if (request.Height is not null)
        {
            if (!Enum.TryParse<KillerHeight>(request.Height, true, out var parsedHeight))
                throw new ArgumentException($"Invalid height value: {request.Height}");
            height = parsedHeight;
        }

        Power? power = null;
        if (request.Power is not null)
            power = new Power(request.Power.Name, request.Power.Description);

        killer.Update(
            name: request.Name,
            realName: request.RealName,
            overview: request.Overview,
            backstory: request.Backstory,
            power: power,
            movementSpeed: request.MovementSpeed,
            terrorRadius: request.TerrorRadius,
            height: height
        );

        if (request.ImageUrl is not null)
            killer.SetImageUrl(request.ImageUrl);

        if (request.ChapterId.HasValue)
            killer.AssignToChapter(request.ChapterId.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return killer.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var killer = await _context.Killers.FindAsync([id], cancellationToken);

        if (killer is null)
            return false;

        _context.Killers.Remove(killer);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
