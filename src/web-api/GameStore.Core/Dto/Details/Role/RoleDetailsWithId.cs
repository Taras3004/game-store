namespace GameStore.Core.Dto.Details.Role
{
    public record RoleDetailsWithId : RoleDetailsBase
    {
        public required Guid Id { get; init; }
    }
}
