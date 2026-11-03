using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Results;
using GameStore.UnitTests.Core.Aggregators.GameProduct.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GameProduct
{
    public class ResolveGameTests : GameProductAggregatorTestsBase
    {
        [Fact]
        public async Task ResolveGame_WhenFoundInGameStore_ReturnsSuccessWithGame()
        {
            var id = Guid.NewGuid().ToString();
            var existingGame = GameTestBuilder.CreateValidGame();
            var expectedId = existingGame.Id;

            GameSourceMock
                .Setup(x => x.GetGameByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync(existingGame);

            var result = await GameProductAggregator.ResolveGame(id);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task ResolveGame_WhenNotFoundAnywhere_ReturnsFailureNotFound()
        {
            var id = "unknown-orderId";
            var expectedError = GameErrors.NotFound;

            GameSourceMock
                .Setup(x => x.GetGameByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync((Game?)null);

            ProductSourceMock
                .Setup(x => x.GetProductEntityById(id, ParameterProvider.AnyToken))
                .ReturnsAsync((NorthwindProduct?)null);

            var result = await GameProductAggregator.ResolveGame(id);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task ResolveGame_WhenGenreResolutionFails_ReturnsGenreNotFoundError()
        {
            var id = "legacy-orderId";
            var rawProduct = ProductTestBuilder.CreateValidNorthwindProduct();
            var migrationData = ProductTestBuilder.CreateValidProductMigrationData();
            var publisherId = Guid.NewGuid();
            var expectedError = GenreErrors.NotFound;

            GameSourceMock
                .Setup(x => x.GetGameByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync((Game?)null);

            ProductSourceMock
                .Setup(x => x.GetProductEntityById(id, ParameterProvider.AnyToken))
                .ReturnsAsync(rawProduct);

            ProductSourceMock
                .Setup(x => x.ExtractMigrationData(rawProduct, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Success(migrationData));

            PublisherSupplierAggregatorMock
                .Setup(x => x.ResolvePublisherId(migrationData.SupplierId, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Success(publisherId));

            GenreCategoryAggregatorMock
                .Setup(x => x.ResolveGenre(migrationData.CategoryId, ParameterProvider.AnyToken))
                .ReturnsAsync((Genre?)null);

            var result = await GameProductAggregator.ResolveGame(id);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task ResolveGame_WhenAllStepsSucceed_ReturnsSuccessWithMigratedGame()
        {
            var id = "legacy-orderId";
            var rawProduct = ProductTestBuilder.CreateValidNorthwindProduct();
            var migrationData = ProductTestBuilder.CreateValidProductMigrationData();
            var publisherId = Guid.NewGuid();
            var genre = GenreTestBuilder.CreateValidGenre();
            var migratedGame = GameTestBuilder.CreateValidGame();
            var expectedId = migratedGame.Id;

            GameSourceMock
                .Setup(x => x.GetGameByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync((Game?)null);

            ProductSourceMock
                .Setup(x => x.GetProductEntityById(id, ParameterProvider.AnyToken))
                .ReturnsAsync(rawProduct);

            ProductSourceMock
                .Setup(x => x.ExtractMigrationData(rawProduct, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Success(migrationData));

            PublisherSupplierAggregatorMock
                .Setup(x => x.ResolvePublisherId(migrationData.SupplierId, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Success(publisherId));

            GenreCategoryAggregatorMock
                .Setup(x => x.ResolveGenre(migrationData.CategoryId, ParameterProvider.AnyToken))
                .ReturnsAsync(genre);

            GameSourceMock
                .Setup(x => x.MigrateProductToGame(migrationData, publisherId, genre, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Success(migratedGame));

            var result = await GameProductAggregator.ResolveGame(id);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }
    }
}