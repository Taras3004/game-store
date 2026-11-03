namespace GameStore.Web.Infrastructure.Settings
{
    public class CacheOptions
    {
        public const string SectionName = "CacheOptions";
        public string TotalGamesKey { get; init; } = string.Empty;
        public int DurationSeconds { get; init; } = 60;
    }
}
