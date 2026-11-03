namespace GameStore.Models.DomainEvents.TransactionalOutbox.MessageTypes
{
    public sealed record SyncStockOutboxMessage(string GameKey, int UnitInStockValue);
}
