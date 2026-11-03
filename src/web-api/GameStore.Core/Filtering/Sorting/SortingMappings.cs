using System.Collections.Frozen;

namespace GameStore.Core.Filtering.Sorting
{
    internal static class SortingMappings
    {
        public static IReadOnlyDictionary<string, GameSortOrder> GameSortingMap { get; } =
            new Dictionary<string, GameSortOrder>(StringComparer.OrdinalIgnoreCase)
            {
                {"Most popular", GameSortOrder.MostPopular},
                {"Most commented", GameSortOrder.MostCommented},
                {"Price ASC", GameSortOrder.PriceAsc},
                {"Price DESC", GameSortOrder.PriceDesc},
                {"New", GameSortOrder.New}
            }.ToFrozenDictionary();
    }
}
