using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PerkRepository : RepositoryBase<Perk>, IPerkRepository
{
    public PerkRepository(DbdContext context) : base(context)
    {
    }

    public override async Task<Perk?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Perk?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Perk>> GetByRoleAsync(Role role, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .Where(p => p.Role == role)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Perk>> GetByKillerIdAsync(Guid killerId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(p => p.Killer)
            .Where(p => p.KillerId == killerId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Perk>> GetBySurvivorIdAsync(Guid survivorId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(p => p.Survivor)
            .Where(p => p.SurvivorId == survivorId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Perk>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(p => p.Killer)
            .Include(p => p.Survivor)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
