using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class StatusEffectRepository : RepositoryBase<StatusEffect>, IStatusEffectRepository
{
    public StatusEffectRepository(DbdContext context) : base(context)
    {
    }

    public async Task<StatusEffect?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<StatusEffect>> GetByTypeAsync(StatusEffectType type, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(s => s.Type == type)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StatusEffect>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(s => s.AppliesTo == role || s.AppliesTo == Role.All)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<StatusEffect>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .OrderBy(s => s.Type)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }
}
