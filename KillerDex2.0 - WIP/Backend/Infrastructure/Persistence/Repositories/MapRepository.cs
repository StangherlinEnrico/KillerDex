using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MapRepository : RepositoryBase<Map>, IMapRepository
{
    public MapRepository(DbdContext context) : base(context)
    {
    }

    public override async Task<Map?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(m => m.Realm)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Map?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(m => m.Realm)
            .FirstOrDefaultAsync(m => m.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Map>> GetByRealmIdAsync(Guid realmId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(m => m.Realm)
            .Where(m => m.RealmId == realmId)
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Map>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(m => m.Realm)
            .OrderBy(m => m.Realm.Name)
            .ThenBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }
}
