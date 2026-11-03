namespace GameStore.Core.Dto.Responses
{
    public record RoleResponse
    {
        public required string Name { get; init; }
        public required Guid Id { get; init; }
    }
}
