namespace Domain.Events;

public sealed class SurvivorCreatedEvent : DomainEvent
{
    public Guid SurvivorId { get; }
    public string SurvivorName { get; }

    public SurvivorCreatedEvent(Guid survivorId, string survivorName)
    {
        SurvivorId = survivorId;
        SurvivorName = survivorName;
    }
}

public sealed class SurvivorUpdatedEvent : DomainEvent
{
    public Guid SurvivorId { get; }

    public SurvivorUpdatedEvent(Guid survivorId)
    {
        SurvivorId = survivorId;
    }
}

public sealed class SurvivorAssignedToChapterEvent : DomainEvent
{
    public Guid SurvivorId { get; }
    public Guid ChapterId { get; }

    public SurvivorAssignedToChapterEvent(Guid survivorId, Guid chapterId)
    {
        SurvivorId = survivorId;
        ChapterId = chapterId;
    }
}
