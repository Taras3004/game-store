namespace GameStore.Core.Dto.Details.User
{
    public record UserLoginBase
    {
        public required string Login { get; init; }
        public required string Password { get; init; }
        public bool InternalAuth { get; init; }
    }
}
