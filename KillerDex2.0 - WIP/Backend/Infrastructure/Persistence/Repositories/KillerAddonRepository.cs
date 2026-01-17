using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class KillerAddonRepository : RepositoryBase<KillerAddon>, IKillerAddonRepository
{
    public KillerAddonRepository(DbdContext context) : base(context)
    {
    }

    public override async Task<KillerAddon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(a => a.Killer)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<KillerAddon?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(a => a.Killer)
            .FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<KillerAddon>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(a => a.Killer)
            .Where(a => a.KillerId == killerId)
            .OrderBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<KillerAddon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(a => a.Killer)
            .OrderBy(a => a.Killer.Name)
            .ThenBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }
}
