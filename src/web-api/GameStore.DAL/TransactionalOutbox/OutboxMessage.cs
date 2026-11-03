namespace GameStore.Models.DomainEvents.TransactionalOutbox
{
    public class OutboxMessage
    {
        public Guid Id { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? CompletedAt { get; private set; }
        public string Type { get; init; }
        public string Payload { get; init; }
        public string? Error { get; private set; }
        public int RetryCount { get; private set; }

        private OutboxMessage()
        {
            Type = null!;
            Payload = null!;
        }

        public OutboxMessage(DateTime currentTime, string type, string payload)
        {
            CreatedDate = currentTime;
            Type = type;
            Payload = payload;
        }

        public void Complete(DateTime currentTime)
        {
            CompletedAt = currentTime;
        }

        public void SetError(string errorMessage)
        {
            Error = errorMessage;
            RetryCount++;
        }
    }
}
