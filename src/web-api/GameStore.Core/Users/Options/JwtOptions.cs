namespace GameStore.Core.Users.Options
{
    public record JwtOptions
    {
        public const string SectionName = "JwtOptions";

        public required string Secret { get; init; }
        public required int ExpirationInMinutes { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
    }
}
