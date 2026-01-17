namespace Domain.Events;

public sealed class KillerCreatedEvent : DomainEvent
{
    public Guid KillerId { get; }
    public string KillerName { get; }

    public KillerCreatedEvent(Guid killerId, string killerName)
    {
        KillerId = killerId;
        KillerName = killerName;
    }
}

public sealed class KillerUpdatedEvent : DomainEvent
{
    public Guid KillerId { get; }

    public KillerUpdatedEvent(Guid killerId)
    {
        KillerId = killerId;
    }
}

public sealed class KillerAssignedToChapterEvent : DomainEvent
{
    public Guid KillerId { get; }
    public Guid ChapterId { get; }

    public KillerAssignedToChapterEvent(Guid killerId, Guid chapterId)
    {
        KillerId = killerId;
        ChapterId = chapterId;
    }
}
