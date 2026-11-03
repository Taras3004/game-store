namespace GameStore.Core.Dto.Responses.AuthService
{
    public record AuthServiceUserResponse
    {
        public required string Email { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
    }
}
