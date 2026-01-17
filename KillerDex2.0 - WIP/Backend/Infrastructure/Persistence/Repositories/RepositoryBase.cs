using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic repository base class implementing common CRUD operations.
/// </summary>
public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbdContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryBase(DbdContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<TEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.ToLowerInvariant().Trim();
        return await DbSet.FirstOrDefaultAsync(e => e.Slug == normalizedSlug, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var totalCount = await DbSet.CountAsync(cancellationToken);

        var items = await DbSet
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, totalCount, page, pageSize);
    }

    public virtual void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    /// <summary>
    /// Returns a queryable for custom queries. Use AsNoTracking() for read-only queries.
    /// </summary>
    protected IQueryable<TEntity> Query(bool trackChanges = false)
    {
        return trackChanges ? DbSet : DbSet.AsNoTracking();
    }
}
