using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class KillerRepository : RepositoryBase<Killer>, IKillerRepository
{
    public KillerRepository(DbdContext context) : base(context)
    {
    }

    public override async Task<Killer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(k => k.Chapter)
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);
    }

    public async Task<Killer?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(k => k.Chapter)
            .FirstOrDefaultAsync(k => k.Name == name, cancellationToken);
    }

    public async Task<Killer?> GetWithAddonsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(k => k.Chapter)
            .Include(k => k.Addons)
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);
    }

    public async Task<Killer?> GetWithPerksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(k => k.Chapter)
            .Include(k => k.Perks)
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Killer>> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(k => k.Chapter)
            .Where(k => k.ChapterId == chapterId)
            .OrderBy(k => k.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Killer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(k => k.Chapter)
            .OrderBy(k => k.Name)
            .ToListAsync(cancellationToken);
    }
}
