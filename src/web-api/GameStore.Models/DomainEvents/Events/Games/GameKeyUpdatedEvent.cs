namespace GameStore.Models.DomainEvents.Events.Games
{
    public sealed record GameKeyUpdatedEvent(Guid Id, string OldKey, string NewKey) : IDomainEvent;
}