using GameStore.Core.Dto.Details.User;

namespace GameStore.Core.Dto.Requests.User
{
    public record LoginUserRequest
    {
        public required UserLoginBase Model { get; init; }
    }
}
