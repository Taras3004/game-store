using GameStore.Core.Dto.Responses;
using GameStore.UnitTests.Core.Aggregators.GameProduct.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GameProduct
{
    public class GetGamesByPublisherCompanyNameTests : GameProductAggregatorTestsBase
    {
        [Fact]
        public async Task GetGamesByPublisherCompanyName_CombinesGamesAndUnmigratedProducts()
        {
            var companyName = "Valve";
            var gameResponse = GameTestBuilder.CreateValidGameResponse(name: "Half-Life");
            var productResponse = GameTestBuilder.CreateValidGameResponse(name: "Portal");
            var gamesList = new List<GameResponse> { gameResponse };
            var legacyIds = new HashSet<string> { "1" };
            var productsList = new List<GameResponse> { productResponse };
            var expectedFirstName = gameResponse.Name;
            var expectedSecondName = productResponse.Name;

            GameSourceMock
                .Setup(x => x.GetGamesByPublisherCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync((gamesList, legacyIds));

            ProductSourceMock
                .Setup(x => x.GetUnmigratedProductsBySupplierCompanyName(companyName, legacyIds, ParameterProvider.AnyToken))
                .ReturnsAsync(productsList);

            var result = (await GameProductAggregator.GetGamesByPublisherCompanyName(companyName)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(expectedFirstName, result.ElementAt(0).Name);
            Assert.Equal(expectedSecondName, result.ElementAt(1).Name);
        }
    }
}