using GameStore.Core.Dto.Responses;
using GameStore.Models.Errors;
using GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using GameStore.UnitTests.Core.Helpers;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier
{
    public class GetPublisherByGameKeyTests : PublisherSupplierAggregatorTestsBase
    {
        [Fact]
        public async Task GetPublisherByGameKey_WhenFoundInGameStore_ReturnsPublisher()
        {
            var gameKey = "the-witcher-3";
            var id = "1";
            var expectedResponse = PublisherTestBuilder.CreateValidPublisherResponse(id: id);
            var expectedId = expectedResponse.Id;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByGameKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync(expectedResponse);

            var result = await PublisherSupplierAggregator.GetPublisherByGameKey(gameKey);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetPublisherByGameKey_WhenNotFoundInGameStoreAndFoundInNorthwind_ReturnsSupplier()
        {
            var gameKey = "half-life-2";
            var expectedResponse = PublisherTestBuilder.CreateValidPublisherResponse();
            var expectedId = expectedResponse.Id;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByGameKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseByProductKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync(expectedResponse);

            var result = await PublisherSupplierAggregator.GetPublisherByGameKey(gameKey);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetPublisherByGameKey_WhenNotFoundAnywhere_ReturnsFailure()
        {
            var gameKey = "unknown-game";
            var expectedError = GameErrors.PublisherNotFound;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByGameKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseByProductKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            var result = await PublisherSupplierAggregator.GetPublisherByGameKey(gameKey);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }
    }
}
