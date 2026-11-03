using GameStore.Core.Dto.Details.MigrationData;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier
{
    public class ResolvePublisherIdTests : PublisherSupplierAggregatorTestsBase
    {
        [Fact]
        public async Task ResolvePublisherId_WhenIdIsGuidAndExists_ReturnsSuccessWithGuid()
        {
            var publisherGuid = Guid.NewGuid();
            var publisherId = publisherGuid.ToString();
            var expectedId = publisherGuid;

            PublisherSourceMock
                .Setup(x => x.Exists(publisherGuid, ParameterProvider.AnyToken))
                .ReturnsAsync(true);

            var result = await PublisherSupplierAggregator.ResolvePublisherId(publisherId);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value);
        }

        [Fact]
        public async Task ResolvePublisherId_WhenIdIsGuidAndDoesNotExist_ReturnsFailureNotFound()
        {
            var publisherGuid = Guid.NewGuid();
            var publisherId = publisherGuid.ToString();
            var expectedError = PublisherErrors.NotFound;

            PublisherSourceMock
                .Setup(x => x.Exists(publisherGuid, ParameterProvider.AnyToken))
                .ReturnsAsync(false);

            var result = await PublisherSupplierAggregator.ResolvePublisherId(publisherId);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task ResolvePublisherId_WhenIdIsLegacyAndSupplierNotFound_ReturnsFailureNotFound()
        {
            var legacyId = "1";
            var expectedError = PublisherErrors.NotFound;

            SupplierSourceMock
                .Setup(x => x.GetSupplierMigrationDataById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync((SupplierMigrationData?)null);

            var result = await PublisherSupplierAggregator.ResolvePublisherId(legacyId);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task ResolvePublisherId_WhenIdIsLegacyAndAlreadyMigrated_ReturnsExistingPublisherId()
        {
            var legacyId = "1";
            var migrationData = PublisherTestBuilder.CreateValidMigrationData(legacyId: legacyId);
            var alreadyMigratedPublisher = PublisherTestBuilder.CreateValidPublisher(legacyId: legacyId);
            var expectedId = alreadyMigratedPublisher.Id;

            SupplierSourceMock
                .Setup(x => x.GetSupplierMigrationDataById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(migrationData);

            PublisherSourceMock
                .Setup(x => x.GetPublisherByLegacyIdAsync(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(alreadyMigratedPublisher);

            var result = await PublisherSupplierAggregator.ResolvePublisherId(legacyId);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value);
        }

        [Fact]
        public async Task ResolvePublisherId_WhenIdIsLegacyAndNotMigrated_MigratesAndReturnsNewPublisherId()
        {
            var legacyId = "1";
            var migrationData = PublisherTestBuilder.CreateValidMigrationData(legacyId: legacyId);
            var migratedPublisher = PublisherTestBuilder.CreateValidPublisher(legacyId: legacyId);
            var expectedId = migratedPublisher.Id;

            SupplierSourceMock
                .Setup(x => x.GetSupplierMigrationDataById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(migrationData);

            PublisherSourceMock
                .Setup(x => x.GetPublisherByLegacyIdAsync(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync((Publisher?)null);

            PublisherSourceMock
                .Setup(x => x.MigrateSupplier(migrationData, ParameterProvider.AnyToken))
                .ReturnsAsync(migratedPublisher);

            var result = await PublisherSupplierAggregator.ResolvePublisherId(legacyId);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value);
        }
    }
}