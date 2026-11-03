namespace GameStore.Core.Dto.Responses
{
    public record LoginResponse
    {
        public required string Token { get; init; }
    }
}
