using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ChapterRepository : RepositoryBase<Chapter>, IChapterRepository
{
    public ChapterRepository(DbdContext context) : base(context)
    {
    }

    public async Task<Chapter?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<Chapter?> GetByNumberAsync(int number, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.Number == number, cancellationToken);
    }

    public async Task<Chapter?> GetWithCharactersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Killers)
            .Include(c => c.Survivors)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Chapter>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(c => c.Number)
            .ToListAsync(cancellationToken);
    }
}
