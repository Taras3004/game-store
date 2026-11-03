namespace GameStore.Core.Users.Options
{
    public class ExternalAuthServiceOptions
    {
        public const string SectionName = "AuthService";

        public required string BaseAddress { get; init; }
    }
}
