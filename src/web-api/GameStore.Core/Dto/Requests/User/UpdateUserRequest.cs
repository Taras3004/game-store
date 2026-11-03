using GameStore.Core.Dto.Details.User;

namespace GameStore.Core.Dto.Requests.User
{
    public record UpdateUserRequest
    {
        public required UserDetailsWithId User { get; init; }
        public IEnumerable<Guid>? Roles { get; init; }
        public required string Password { get; init; }
    }
}
