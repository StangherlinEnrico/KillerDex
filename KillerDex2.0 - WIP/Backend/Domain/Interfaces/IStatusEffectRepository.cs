using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IStatusEffectRepository : IRepository<StatusEffect>
{
    Task<StatusEffect?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<StatusEffect>> GetByTypeAsync(StatusEffectType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<StatusEffect>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default);
}
