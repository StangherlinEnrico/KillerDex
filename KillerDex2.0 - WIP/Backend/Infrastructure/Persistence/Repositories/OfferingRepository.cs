using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OfferingRepository : RepositoryBase<Offering>, IOfferingRepository
{
    public OfferingRepository(DbdContext context) : base(context)
    {
    }

    public async Task<Offering?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(o => o.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Offering>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(o => o.Role == role)
            .OrderBy(o => o.Rarity)
            .ThenBy(o => o.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Offering>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(o => o.Role)
            .ThenBy(o => o.Rarity)
            .ThenBy(o => o.Name)
            .ToListAsync(cancellationToken);
    }
}
