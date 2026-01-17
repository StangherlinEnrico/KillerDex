using Domain.Entities;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Interceptors;

/// <summary>
/// Interceptor that dispatches domain events after saving changes.
/// Events are collected from entities and dispatched after successful save.
/// </summary>
public class DomainEventDispatcherInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcherInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        return result;
    }

    public override int SavedChanges(
        SaveChangesCompletedEventData eventData,
        int result)
    {
        DispatchDomainEventsAsync(eventData.Context, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        return result;
    }

    private async Task DispatchDomainEventsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null) return;

        var entitiesWithEvents = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Clear events from entities
        foreach (var entity in entitiesWithEvents)
        {
            entity.ClearDomainEvents();
        }

        // Dispatch events
        foreach (var domainEvent in domainEvents)
        {
            await DispatchEventAsync(domainEvent, cancellationToken);
        }
    }

    private async Task DispatchEventAsync(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var eventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            if (handler is null) continue;

            var method = handlerType.GetMethod(nameof(IDomainEventHandler<DomainEvent>.HandleAsync));
            if (method is not null)
            {
                var task = (Task?)method.Invoke(handler, [domainEvent, cancellationToken]);
                if (task is not null)
                {
                    await task;
                }
            }
        }
    }
}
