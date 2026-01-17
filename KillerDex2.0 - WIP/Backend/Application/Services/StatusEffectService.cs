using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
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
}
