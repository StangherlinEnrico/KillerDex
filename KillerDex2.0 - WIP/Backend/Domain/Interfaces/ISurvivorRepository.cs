using Domain.Entities;

namespace Domain.Interfaces;

public interface ISurvivorRepository : IRepository<Survivor>
{
    Task<Survivor?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Survivor?> GetWithPerksAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Survivor>> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default);
}
