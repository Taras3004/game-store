using GameStore.Core.Dto.Responses;
using GameStore.UnitTests.Core.Aggregators.GameProduct.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GameProduct
{
    public class GetGamesByGenreIdTests : GameProductAggregatorTestsBase
    {
        [Fact]
        public async Task GetGamesByGenreId_WhenIdIsGuid_ReturnsFromGameSource()
        {
            var guid = Guid.NewGuid();
            var genreId = guid.ToString();
            var setupResponse = GameTestBuilder.CreateValidGameResponse();
            var expectedList = new List<GameResponse> { setupResponse };
            var expectedId = setupResponse.Id;

            GameSourceMock
                .Setup(x => x.GetGamesByGenreId(guid, ParameterProvider.AnyToken))
                .ReturnsAsync(expectedList);

            var result = (await GameProductAggregator.GetGamesByGenreId(genreId)).ToList();

            Assert.Single(result);
            Assert.Equal(expectedId, result.First().Id);
        }

        [Fact]
        public async Task GetGamesByGenreId_WhenIdIsLegacy_ReturnsFromProductSource()
        {
            var legacyId = "1";
            var setupResponse = GameTestBuilder.CreateValidGameResponse();
            var expectedList = new List<GameResponse> { setupResponse };
            var expectedId = setupResponse.Id;

            ProductSourceMock
                .Setup(x => x.GetProductsByCategoryId(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(expectedList);

            var result = (await GameProductAggregator.GetGamesByGenreId(legacyId)).ToList();

            Assert.Single(result);
            Assert.Equal(expectedId, result.First().Id);
        }
    }
}