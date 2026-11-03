using GameStore.Core.Dto.Details.User;

namespace GameStore.Core.Dto.Requests.User
{
    public record CreateUserRequest
    {
        public required UserDetailsBase User { get; init; }
        public IEnumerable<Guid>? Roles { get; init; }
        public required string Password { get; init; }
    }
}
