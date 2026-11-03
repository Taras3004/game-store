namespace GameStore.Core.Dto.Requests.User
{
    public record UserAccessRequest
    {
        public required string TargetPage { get; init; }
        public required Guid TargetId { get; init; }
    }
}
