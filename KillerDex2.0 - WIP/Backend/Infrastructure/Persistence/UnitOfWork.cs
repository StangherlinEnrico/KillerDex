using Domain.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation that coordinates multiple repository operations
/// within a single transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbdContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    // Lazy-loaded repositories
    private IKillerRepository? _killers;
    private ISurvivorRepository? _survivors;
    private IChapterRepository? _chapters;
    private IPerkRepository? _perks;
    private IKillerAddonRepository? _killerAddons;
    private ISurvivorAddonRepository? _survivorAddons;
    private IItemRepository? _items;
    private IOfferingRepository? _offerings;
    private IRealmRepository? _realms;
    private IMapRepository? _maps;
    private IStatusEffectRepository? _statusEffects;

    public UnitOfWork(DbdContext context)
    {
        _context = context;
    }

    public IKillerRepository Killers => _killers ??= new KillerRepository(_context);
    public ISurvivorRepository Survivors => _survivors ??= new SurvivorRepository(_context);
    public IChapterRepository Chapters => _chapters ??= new ChapterRepository(_context);
    public IPerkRepository Perks => _perks ??= new PerkRepository(_context);
    public IKillerAddonRepository KillerAddons => _killerAddons ??= new KillerAddonRepository(_context);
    public ISurvivorAddonRepository SurvivorAddons => _survivorAddons ??= new SurvivorAddonRepository(_context);
    public IItemRepository Items => _items ??= new ItemRepository(_context);
    public IOfferingRepository Offerings => _offerings ??= new OfferingRepository(_context);
    public IRealmRepository Realms => _realms ??= new RealmRepository(_context);
    public IMapRepository Maps => _maps ??= new MapRepository(_context);
    public IStatusEffectRepository StatusEffects => _statusEffects ??= new StatusEffectRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("No transaction has been started. Call BeginTransactionAsync first.");

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
        }
        await _context.DisposeAsync();
    }
}
