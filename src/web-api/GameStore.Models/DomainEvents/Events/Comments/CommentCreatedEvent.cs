namespace GameStore.Models.DomainEvents.Events.Comments
{
    public record CommentCreatedEvent(string GameKey) : IDomainEvent;
}
