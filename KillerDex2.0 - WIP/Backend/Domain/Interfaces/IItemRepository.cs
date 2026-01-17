using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Item>> GetByTypeAsync(ItemType type, CancellationToken cancellationToken = default);
}
