using Domain.Entities;

namespace Domain.Interfaces;

public interface IMapRepository : IRepository<Map>
{
    Task<Map?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Map>> GetByRealmIdAsync(Guid realmId, CancellationToken cancellationToken = default);
}
