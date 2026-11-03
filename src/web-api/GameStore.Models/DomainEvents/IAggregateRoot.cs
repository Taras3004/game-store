namespace GameStore.Models.DomainEvents
{
    public interface IAggregateRoot
    {
        List<IDomainEvent> DomainEvents { get; }
        void Raise(IDomainEvent domainEvent);
        void ClearDomainEvents();
    }
}
