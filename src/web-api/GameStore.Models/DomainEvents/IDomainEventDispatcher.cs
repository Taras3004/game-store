namespace GameStore.Models.DomainEvents
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken token = default);
    }
}
