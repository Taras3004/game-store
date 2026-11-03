using GameStore.Core.Dto.Responses;
using GameStore.Models.Errors;
using GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier
{
    public class GetPublisherByIdAsyncTests : PublisherSupplierAggregatorTestsBase
    {
        [Fact]
        public async Task GetPublisherByIdAsync_WhenIdIsGuidAndExists_ReturnsSuccess()
        {
            var guid = Guid.NewGuid();
            var setupResponse = PublisherTestBuilder.CreateValidPublisherResponse(id: guid.ToString());
            var expectedId = setupResponse.Id;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseById(guid, ParameterProvider.AnyToken))
                .ReturnsAsync(setupResponse);

            var result = await PublisherSupplierAggregator.GetPublisherById(guid.ToString());

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_WhenIdIsGuidAndDoesNotExist_ReturnsFailure()
        {
            var guid = Guid.NewGuid();
            var expectedError = PublisherErrors.NotFound;

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseById(guid, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            var result = await PublisherSupplierAggregator.GetPublisherById(guid.ToString());

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_WhenIdIsLegacyAndDoesNotExist_ReturnsFailure()
        {
            var legacyId = "1";
            var expectedError = PublisherErrors.NotFound;

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            var result = await PublisherSupplierAggregator.GetPublisherById(legacyId);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_WhenIdIsLegacyAndAlreadyMigrated_ReturnsMigratedPublisher()
        {
            var legacyId = "1";
            var companyName = "SomeCompany";
            var supplierResponse = PublisherTestBuilder.CreateValidPublisherResponse(id: legacyId, companyName: companyName);
            var migratedPublisherResponse = PublisherTestBuilder.CreateValidPublisherResponse(companyName: companyName);
            var expectedId = migratedPublisherResponse.Id;

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(supplierResponse);

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync(migratedPublisherResponse);

            var result = await PublisherSupplierAggregator.GetPublisherById(legacyId);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_WhenIdIsLegacyAndNotMigrated_ReturnsSupplierResponse()
        {
            var legacyId = "1";
            var companyName = "SomeCompany";
            var supplierResponse = PublisherTestBuilder.CreateValidPublisherResponse(id: legacyId, companyName: companyName);
            var expectedId = supplierResponse.Id;

            SupplierSourceMock
                .Setup(x => x.GetSupplierResponseById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(supplierResponse);

            PublisherSourceMock
                .Setup(x => x.GetPublisherResponseByCompanyName(companyName, ParameterProvider.AnyToken))
                .ReturnsAsync((PublisherResponse?)null);

            var result = await PublisherSupplierAggregator.GetPublisherById(legacyId);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }
    }
}