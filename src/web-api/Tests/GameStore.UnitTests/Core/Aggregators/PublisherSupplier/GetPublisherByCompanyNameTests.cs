using GameStore.Core.Dto.Responses;
using GameStore.Models.Errors;
using GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier
{
    public class GetPublisherByCompanyNameTests : PublisherSupplierAggregatorTestsBase
    {
        [Fact]
        public async Task GetPublisherByCompanyName_WhenFoundInGameStore_ReturnsPublisher()
        {
            var companyName = "SomeCompany";
            var expectedResponse = PublisherTestBuilder.CreateValidPublisherResponse(companyName: companyName);
            var expectedId = expectedResponse.Id;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync(expectedResponse);

            var result = await PublisherSupplierAggregator.GetPublisherByCompanyName(companyName);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetPublisherByCompanyName_WhenNotFoundInGameStoreAndFoundInNorthwind_ReturnsSupplier()
        {
            var companyName = "SomeCompany";
            var expectedResponse = PublisherTestBuilder.CreateValidPublisherResponse(companyName: companyName);
            var expectedId = expectedResponse.Id;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync(expectedResponse);

            var result = await PublisherSupplierAggregator.GetPublisherByCompanyName(companyName);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetPublisherByCompanyName_WhenNotFoundAnywhere_ReturnsFailure()
        {
            var companyName = "Unknown";
            var expectedError = PublisherErrors.NotFound;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            var result = await PublisherSupplierAggregator.GetPublisherByCompanyName(companyName);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }
    }
}