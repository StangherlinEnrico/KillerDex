using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class StatusEffectService : IStatusEffectService
{
    private readonly DbdContext _context;

    public StatusEffectService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StatusEffectSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var effects = await _context.StatusEffects
            .OrderBy(s => s.Type)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return effects.Select(s => s.ToSummaryDto());
    }

    public async Task<IEnumerable<StatusEffectSummaryDto>> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<StatusEffectType>(type, true, out var typeEnum))
            return Enumerable.Empty<StatusEffectSummaryDto>();

        var effects = await _context.StatusEffects
            .Where(s => s.Type == typeEnum)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return effects.Select(s => s.ToSummaryDto());
    }

    public async Task<StatusEffectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var effect = await _context.StatusEffects
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return effect?.ToDto();
    }

    public async Task<StatusEffectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var effect = await _context.StatusEffects
            .FirstOrDefaultAsync(s => s.Slug == slug.ToLowerInvariant(), cancellationToken);

        return effect?.ToDto();
    }

    public async Task<StatusEffectDto> CreateAsync(CreateStatusEffectRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<StatusEffectType>(request.Type, true, out var type))
            throw new ArgumentException($"Invalid status effect type: {request.Type}");

        if (!Enum.TryParse<Role>(request.AppliesTo, true, out var appliesTo))
            throw new ArgumentException($"Invalid role: {request.AppliesTo}");

        var effect = new StatusEffect(
            name: request.Name,
            type: type,
            appliesTo: appliesTo,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            effect.SetImageUrl(request.ImageUrl);

        _context.StatusEffects.Add(effect);
        await _context.SaveChangesAsync(cancellationToken);

        return effect.ToDto();
    }

    public async Task<StatusEffectDto?> UpdateAsync(Guid id, UpdateStatusEffectRequest request, CancellationToken cancellationToken = default)
    {
        var effect = await _context.StatusEffects.FindAsync([id], cancellationToken);
        if (effect is null) return null;

        StatusEffectType? type = null;
        if (request.Type is not null && Enum.TryParse<StatusEffectType>(request.Type, true, out var parsedType))
            type = parsedType;

        Role? appliesTo = null;
        if (request.AppliesTo is not null && Enum.TryParse<Role>(request.AppliesTo, true, out var parsedAppliesTo))
            appliesTo = parsedAppliesTo;

        effect.Update(name: request.Name, description: request.Description, type: type, appliesTo: appliesTo);

        if (request.ImageUrl is not null)
            effect.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            effect.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);
        return effect.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var effect = await _context.StatusEffects.FindAsync([id], cancellationToken);
        if (effect is null) return false;

        _context.StatusEffects.Remove(effect);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
