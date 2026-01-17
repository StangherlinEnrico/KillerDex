using Domain.Entities;

namespace Domain.Interfaces;

public interface IChapterRepository : IRepository<Chapter>
{
    Task<Chapter?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Chapter?> GetByNumberAsync(int number, CancellationToken cancellationToken = default);
    Task<Chapter?> GetWithCharactersAsync(Guid id, CancellationToken cancellationToken = default);
}
