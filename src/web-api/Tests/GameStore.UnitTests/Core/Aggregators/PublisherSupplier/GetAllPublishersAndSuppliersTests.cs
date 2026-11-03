using GameStore.Core.Dto.Responses;
using GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using GameStore.UnitTests.Core.Helpers;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier
{
    public class GetAllPublishersAndSuppliersTests : PublisherSupplierAggregatorTestsBase
    {
        [Fact]
        public async Task GetAllPublishersAndSuppliers_FiltersOutMigratedSuppliers_ReturnsCombinedSortedList()
        {
            var name1 = "Capcom";
            var name2 = "CD Projekt Red";
            var name2Lower = "cd projekt red";
            var name3 = "Konami";

            var publishers = new List<PublisherResponse>
            {
                PublisherTestBuilder.CreateValidPublisherResponse(companyName: name1),
                PublisherTestBuilder.CreateValidPublisherResponse(companyName: name2)
            };

            var suppliers = new List<PublisherResponse>
            {
                PublisherTestBuilder.CreateValidPublisherResponse(companyName: name2Lower),
                PublisherTestBuilder.CreateValidPublisherResponse(companyName: name3)
            };

            PublisherSourceMock
                .Setup(x => x.GetAllPublishersAsync(ParameterProvider.AnyToken))
                .ReturnsAsync(publishers);

            SupplierSourceMock
                .Setup(x => x.GetAllSupplierResponses(ParameterProvider.AnyToken))
                .ReturnsAsync(suppliers);

            var result = await PublisherSupplierAggregator.GetAllPublishersAndSuppliers();

            var resultList = result.ToList();

            Assert.Equal(3, resultList.Count);
            Assert.Equal(name1, resultList[0].CompanyName);
            Assert.Equal(name2, resultList[1].CompanyName);
            Assert.Equal(name3, resultList[2].CompanyName);
        }
    }
}
