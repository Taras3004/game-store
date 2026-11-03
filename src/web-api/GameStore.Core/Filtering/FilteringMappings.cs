using System.Collections.Frozen;

namespace GameStore.Core.Filtering
{
    public static class FilteringMappings
    {
        public static IReadOnlyDictionary<string, int> PaginationMap { get; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            {"10", 10},
            {"20", 20},
            {"50", 50},
            {"100", 100},
            {"all", int.MaxValue}
        }.ToFrozenDictionary();

        public static IReadOnlyDictionary<string, TimeSpan> DateFilteringMap { get; } = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
        {   
            {"last week", TimeSpan.FromDays(7)},
            {"last month", TimeSpan.FromDays(30)},
            {"last year", TimeSpan.FromDays(365)},
            {"2 years", TimeSpan.FromDays(730)},
            {"3 years", TimeSpan.FromDays(1095)}
        }.ToFrozenDictionary();
    }
}