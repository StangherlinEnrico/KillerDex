using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface ISurvivorAddonRepository : IRepository<SurvivorAddon>
{
    Task<SurvivorAddon?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<SurvivorAddon>> GetByItemTypeAsync(ItemType itemType, CancellationToken cancellationToken = default);
}
