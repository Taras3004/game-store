using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Dto.Details.MigrationData;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class ProductTestBuilder
    {
        public static ProductMigrationData CreateValidProductMigrationData(
            string legacyId = EntityBuilderConstants.ProvidedByBuilderString,
            GameDetailsBase? gameDetails = null,
            string gameKey = EntityBuilderConstants.ProvidedByBuilderString,
            string supplierId = EntityBuilderConstants.ProvidedByBuilderString,
            string categoryId = EntityBuilderConstants.ProvidedByBuilderString)
        {
            gameDetails ??= GameTestBuilder.CreateValidGameDetailsBase();
            return new ProductMigrationData()
            {
                LegacyId = legacyId,
                GameDetails = gameDetails,
                SupplierId = supplierId,
                CategoryId = categoryId,
            };
        }

        public static NorthwindProduct CreateValidNorthwindProduct(
            string id = EntityBuilderConstants.ProvidedByBuilderString,
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string key = EntityBuilderConstants.ProvidedByBuilderString
            )
        {
            return new NorthwindProduct()
            {
                Id = id,
                ProductName = name,
                ProductKey = key
            };
        }
    }
}
