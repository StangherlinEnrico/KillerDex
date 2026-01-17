using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IPerkRepository : IRepository<Perk>
{
    Task<Perk?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Perk>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default);
    Task<IEnumerable<Perk>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Perk>> GetBySurvivorIdAsync(Guid survivorId, CancellationToken cancellationToken = default);
}
