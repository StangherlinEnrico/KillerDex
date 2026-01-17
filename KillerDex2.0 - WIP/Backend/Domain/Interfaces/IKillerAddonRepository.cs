using Domain.Entities;

namespace Domain.Interfaces;

public interface IKillerAddonRepository : IRepository<KillerAddon>
{
    Task<KillerAddon?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<KillerAddon>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default);
}
