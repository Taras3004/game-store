using GameStore.Core.Services.Aggregators;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base
{
    public abstract class PublisherSupplierAggregatorTestsBase
    {
        internal readonly Mock<IPublisherSource> PublisherSourceMock = new();
        internal readonly Mock<ISupplierSource> SupplierSourceMock = new();

        internal readonly PublisherSupplierAggregator PublisherSupplierAggregator;

        protected PublisherSupplierAggregatorTestsBase()
        {
            PublisherSupplierAggregator = new(PublisherSourceMock.Object, SupplierSourceMock.Object);
        }
    }
}
