using GameStore.Core.Dto.Details.Game;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class FilterTestBuilder
    {
        public static GameFilterDetails CreateValidGameFilterDetails(
            string? name = null,
            IEnumerable<string>? genres = null,
            IEnumerable<Guid>? platforms = null,
            IEnumerable<string>? publishers = null,
            double? minPrice = null,
            double? maxPrice = null,
            string? datePublishing = null,
            string? pageCount = null,
            int? page = null,
            string? sort = null)
        {
            return new GameFilterDetails()
            {
                Name = name,
                Genres = genres,
                Platforms = platforms,
                Publishers = publishers,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                DatePublishing = datePublishing,
                PageCount = pageCount,
                Page = page,
                Sort = sort
            };
        }
    }
}
