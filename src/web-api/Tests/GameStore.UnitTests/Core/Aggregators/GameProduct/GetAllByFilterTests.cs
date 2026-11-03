using GameStore.Core.Dto.Responses;
using GameStore.UnitTests.Core.Aggregators.GameProduct.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GameProduct
{
    public class GetAllByFilterTests : GameProductAggregatorTestsBase
    {
        [Fact]
        public async Task GetAllByFilter_WhenNoSortOrPagination_CombinesAndReturnsDefaultPage()
        {
            var filter = FilterTestBuilder.CreateValidGameFilterDetails();
            var gameResponse = GameTestBuilder.CreateValidGameResponse(key: "key");
            var productResponse = GameTestBuilder.CreateValidGameResponse(key: "random-key");
            var gamesList = new List<GameResponse> { gameResponse };
            var productsList = new List<GameResponse> { productResponse };
            var expectedCurrentPage = 1;
            var expectedTotalPages = 1;

            GameSourceMock
                .Setup(x => x.GetGamesByFilter(filter, ParameterProvider.AnyToken))
                .ReturnsAsync(gamesList);

            ProductSourceMock
                .Setup(x => x.GetProductResponsesByFilter(filter, ParameterProvider.AnyToken))
                .ReturnsAsync(productsList);

            var result = await GameProductAggregator.GetAllByFilter(filter);

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Games.Count());
            Assert.Equal(expectedCurrentPage, result.Value.CurrentPage);
            Assert.Equal(expectedTotalPages, result.Value.TotalPages);
        }
    }
}