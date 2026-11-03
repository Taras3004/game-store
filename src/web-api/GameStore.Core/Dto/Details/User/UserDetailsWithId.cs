namespace GameStore.Core.Dto.Details.User
{
    public record UserDetailsWithId : UserDetailsBase
    {
        public Guid Id { get; init; }
    }
}
