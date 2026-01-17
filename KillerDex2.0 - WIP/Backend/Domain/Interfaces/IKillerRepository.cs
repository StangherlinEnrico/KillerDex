using Domain.Entities;

namespace Domain.Interfaces;

public interface IKillerRepository : IRepository<Killer>
{
    Task<Killer?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Killer?> GetWithAddonsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Killer?> GetWithPerksAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Killer>> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default);
}
