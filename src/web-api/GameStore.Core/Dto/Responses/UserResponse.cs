namespace GameStore.Core.Dto.Responses
{
    public record UserResponse
    {
        public required string Name { get; init; }
        public required Guid Id { get; init; }
    }
}
