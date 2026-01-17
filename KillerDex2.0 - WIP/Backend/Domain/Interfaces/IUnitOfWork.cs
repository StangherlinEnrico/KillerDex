namespace Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface for coordinating multiple repository operations
/// within a single transaction.
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IKillerRepository Killers { get; }
    ISurvivorRepository Survivors { get; }
    IChapterRepository Chapters { get; }
    IPerkRepository Perks { get; }
    IKillerAddonRepository KillerAddons { get; }
    ISurvivorAddonRepository SurvivorAddons { get; }
    IItemRepository Items { get; }
    IOfferingRepository Offerings { get; }
    IRealmRepository Realms { get; }
    IMapRepository Maps { get; }
    IStatusEffectRepository StatusEffects { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
