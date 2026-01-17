using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IOfferingRepository : IRepository<Offering>
{
    Task<Offering?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Offering>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default);
}
