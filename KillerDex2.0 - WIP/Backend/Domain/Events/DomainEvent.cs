namespace Domain.Events;

/// <summary>
/// Base class for all domain events.
/// Domain events represent something that happened in the domain that other parts of the system might be interested in.
/// </summary>
public abstract class DomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Marker interface for domain event handlers.
/// </summary>
public interface IDomainEventHandler<in TEvent> where TEvent : DomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
