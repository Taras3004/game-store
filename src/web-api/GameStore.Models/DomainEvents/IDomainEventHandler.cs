namespace GameStore.Models.DomainEvents
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        Task Handle(T domainEvent, CancellationToken  cancellationToken);
    }
}
