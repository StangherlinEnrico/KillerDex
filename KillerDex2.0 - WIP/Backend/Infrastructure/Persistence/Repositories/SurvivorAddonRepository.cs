using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SurvivorAddonRepository : RepositoryBase<SurvivorAddon>, ISurvivorAddonRepository
{
    public SurvivorAddonRepository(DbdContext context) : base(context)
    {
    }

    public async Task<SurvivorAddon?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<SurvivorAddon>> GetByItemTypeAsync(ItemType itemType, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(a => a.ItemType == itemType)
            .OrderBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<SurvivorAddon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(a => a.ItemType)
            .ThenBy(a => a.Rarity)
            .ThenBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }
}
