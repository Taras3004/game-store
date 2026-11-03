namespace GameStore.Models.DomainEvents.TransactionalOutbox.MessageTypes
{
    public sealed record LogOutboxMessage(
        string EntityName, 
        string Action, 
        string OldVersion, 
        string NewVersion, 
        DateTime CreatedDate);
}
