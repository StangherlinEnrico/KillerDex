using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RealmRepository : RepositoryBase<Realm>, IRealmRepository
{
    public RealmRepository(DbdContext context) : base(context)
    {
    }

    public override async Task<Realm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Killer)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Realm?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Killer)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<Realm?> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Killer)
            .FirstOrDefaultAsync(r => r.KillerId == killerId, cancellationToken);
    }

    public async Task<Realm?> GetWithMapsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Killer)
            .Include(r => r.Maps)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Realm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(r => r.Killer)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}
