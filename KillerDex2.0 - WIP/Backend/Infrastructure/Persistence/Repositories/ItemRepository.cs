using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ItemRepository : RepositoryBase<Item>, IItemRepository
{
    public ItemRepository(DbdContext context) : base(context)
    {
    }

    public async Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(i => i.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Item>> GetByTypeAsync(ItemType type, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(i => i.Type == type)
            .OrderBy(i => i.Rarity)
            .ThenBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Item>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(i => i.Type)
            .ThenBy(i => i.Rarity)
            .ThenBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }
}
