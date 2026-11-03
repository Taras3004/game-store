using GameStore.Core.Services.Aggregators;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GameProduct.Base
{
    public class GameProductAggregatorTestsBase
    {
        internal readonly Mock<IGameSource> GameSourceMock = new();
        internal readonly Mock<IProductSource> ProductSourceMock = new();
        internal readonly Mock<IGenreCategoryAggregator> GenreCategoryAggregatorMock = new();
        internal readonly Mock<IPublisherSupplierAggregator> PublisherSupplierAggregatorMock = new();
        internal readonly Mock<ILogger<GameProductAggregator>> LoggerMock = new();

        internal readonly GameProductAggregator GameProductAggregator;

        protected GameProductAggregatorTestsBase()
        {
            GameProductAggregator = new(
                GameSourceMock.Object, 
                ProductSourceMock.Object, 
                GenreCategoryAggregatorMock.Object, 
                PublisherSupplierAggregatorMock.Object,
                LoggerMock.Object);
        }
    }
}
