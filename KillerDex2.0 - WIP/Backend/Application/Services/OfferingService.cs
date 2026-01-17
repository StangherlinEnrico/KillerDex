using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class OfferingService : IOfferingService
{
    private readonly DbdContext _context;

    public OfferingService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OfferingSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var offerings = await _context.Offerings
            .OrderBy(o => o.Role)
            .ThenBy(o => o.Rarity)
            .ThenBy(o => o.Name)
            .ToListAsync(cancellationToken);

        return offerings.Select(o => o.ToSummaryDto());
    }

    public async Task<IEnumerable<OfferingSummaryDto>> GetByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Role>(role, true, out var roleEnum))
            return Enumerable.Empty<OfferingSummaryDto>();

        var offerings = await _context.Offerings
            .Where(o => o.Role == roleEnum || o.Role == Role.All)
            .OrderBy(o => o.Rarity)
            .ThenBy(o => o.Name)
            .ToListAsync(cancellationToken);

        return offerings.Select(o => o.ToSummaryDto());
    }

    public async Task<OfferingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offering = await _context.Offerings
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return offering?.ToDto();
    }

    public async Task<OfferingDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var offering = await _context.Offerings
            .FirstOrDefaultAsync(o => o.Slug == slug.ToLowerInvariant(), cancellationToken);

        return offering?.ToDto();
    }
}
