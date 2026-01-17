using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;

/// <summary>
/// Represents a perk in Dead by Daylight.
/// Perks must belong to either a Killer or a Survivor, never both (Role.All is not allowed).
/// </summary>
public class Perk : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Role Role { get; private set; }
    public Guid? KillerId { get; private set; }
    public Killer? Killer { get; private set; }
    public Guid? SurvivorId { get; private set; }
    public Survivor? Survivor { get; private set; }

    private Perk() { }

    public Perk(string name, Role role, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Perk");
        ValidateDescription(description, "Perk");
        ValidatePerkRole(role);

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Role = role;
        GameVersion = gameVersion;

        AddDomainEvent(new PerkCreatedEvent(Id, name));
    }

    public void Update(string? name = null, string? description = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Perk");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Perk");
            Description = description;
        }

        MarkAsUpdated();
    }

    public void AssignToKiller(Guid killerId)
    {
        if (Role != Role.Killer)
            throw new InvalidOperationException("Only killer perks can be assigned to a killer.");

        SurvivorId = null;
        KillerId = killerId;
        MarkAsUpdated();
        AddDomainEvent(new PerkAssignedToKillerEvent(Id, killerId));
    }

    public void AssignToSurvivor(Guid survivorId)
    {
        if (Role != Role.Survivor)
            throw new InvalidOperationException("Only survivor perks can be assigned to a survivor.");

        KillerId = null;
        SurvivorId = survivorId;
        MarkAsUpdated();
        AddDomainEvent(new PerkAssignedToSurvivorEvent(Id, survivorId));
    }

    private static void ValidatePerkRole(Role role)
    {
        if (role == Role.All)
            throw new ArgumentException("Perk must belong to Killer or Survivor, not All. Use Role.Killer or Role.Survivor.", nameof(role));
    }
}
