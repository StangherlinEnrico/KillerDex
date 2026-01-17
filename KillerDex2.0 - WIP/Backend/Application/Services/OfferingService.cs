using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
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

    public async Task<OfferingDto> CreateAsync(CreateOfferingRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Rarity>(request.Rarity, true, out var rarity))
            throw new ArgumentException($"Invalid rarity: {request.Rarity}");

        if (!Enum.TryParse<Role>(request.Role, true, out var role))
            throw new ArgumentException($"Invalid role: {request.Role}");

        var offering = new Offering(
            name: request.Name,
            rarity: rarity,
            role: role,
            description: request.Description,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            offering.SetImageUrl(request.ImageUrl);

        _context.Offerings.Add(offering);
        await _context.SaveChangesAsync(cancellationToken);

        return offering.ToDto();
    }

    public async Task<OfferingDto?> UpdateAsync(Guid id, UpdateOfferingRequest request, CancellationToken cancellationToken = default)
    {
        var offering = await _context.Offerings.FindAsync([id], cancellationToken);
        if (offering is null) return null;

        Rarity? rarity = null;
        if (request.Rarity is not null && Enum.TryParse<Rarity>(request.Rarity, true, out var parsedRarity))
            rarity = parsedRarity;

        Role? role = null;
        if (request.Role is not null && Enum.TryParse<Role>(request.Role, true, out var parsedRole))
            role = parsedRole;

        offering.Update(name: request.Name, description: request.Description, rarity: rarity, role: role);

        if (request.ImageUrl is not null)
            offering.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            offering.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);
        return offering.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offering = await _context.Offerings.FindAsync([id], cancellationToken);
        if (offering is null) return false;

        _context.Offerings.Remove(offering);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
