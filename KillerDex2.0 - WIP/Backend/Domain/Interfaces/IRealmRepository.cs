using Domain.Entities;

namespace Domain.Interfaces;

public interface IRealmRepository : IRepository<Realm>
{
    Task<Realm?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Realm?> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default);
    Task<Realm?> GetWithMapsAsync(Guid id, CancellationToken cancellationToken = default);
}
