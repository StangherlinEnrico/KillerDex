using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SurvivorRepository : RepositoryBase<Survivor>, ISurvivorRepository
{
    public SurvivorRepository(DbdContext context) : base(context)
    {
    }

    public override async Task<Survivor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Chapter)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Survivor?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Chapter)
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }

    public async Task<Survivor?> GetWithPerksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Chapter)
            .Include(s => s.Perks)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Survivor>> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(s => s.Chapter)
            .Where(s => s.ChapterId == chapterId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Survivor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(s => s.Chapter)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }
}
