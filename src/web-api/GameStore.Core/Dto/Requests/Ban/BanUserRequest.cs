namespace GameStore.Core.Dto.Requests.Ban
{
    public record BanUserRequest
    {
        public required string User { get; init; }
        public required string Duration { get; init; }
    }
}
