namespace Domain.Events;

public sealed class PerkCreatedEvent : DomainEvent
{
    public Guid PerkId { get; }
    public string PerkName { get; }

    public PerkCreatedEvent(Guid perkId, string perkName)
    {
        PerkId = perkId;
        PerkName = perkName;
    }
}

public sealed class PerkAssignedToKillerEvent : DomainEvent
{
    public Guid PerkId { get; }
    public Guid KillerId { get; }

    public PerkAssignedToKillerEvent(Guid perkId, Guid killerId)
    {
        PerkId = perkId;
        KillerId = killerId;
    }
}

public sealed class PerkAssignedToSurvivorEvent : DomainEvent
{
    public Guid PerkId { get; }
    public Guid SurvivorId { get; }

    public PerkAssignedToSurvivorEvent(Guid perkId, Guid survivorId)
    {
        PerkId = perkId;
        SurvivorId = survivorId;
    }
}
