using GameStore.Models.Entities;
using GameStore.Core.Dto.Details.MigrationData;
using GameStore.UnitTests.Core.Aggregators.PublisherSupplier.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.PublisherSupplier
{
    public class ResolvePublisherTests : PublisherSupplierAggregatorTestsBase
    {
        [Fact]
        public async Task ResolvePublisher_WhenFoundInGameStore_ReturnsPublisher()
        {
            var id = Guid.NewGuid().ToString();
            var existingPublisher = PublisherTestBuilder.CreateValidPublisher(id: Guid.Parse(id));
            var expectedId = existingPublisher.Id;

            PublisherSourceMock
                .Setup(x => x.GetPublisherByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync(existingPublisher);

            var result = await PublisherSupplierAggregator.ResolvePublisher(id);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
        }

        [Fact]
        public async Task ResolvePublisher_WhenNotFoundInBothSources_ReturnsNull()
        {
            var id = "nonExistentId";

            PublisherSourceMock
                .Setup(x => x.GetPublisherByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync((Publisher?)null);

            SupplierSourceMock
                .Setup(x => x.GetSupplierMigrationDataById(id, ParameterProvider.AnyToken))
                .ReturnsAsync((SupplierMigrationData?)null);

            var result = await PublisherSupplierAggregator.ResolvePublisher(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task ResolvePublisher_WhenFoundOnlyInNorthwind_MigratesAndReturnsPublisher()
        {
            var legacyId = "1";
            var rawSupplier = PublisherTestBuilder.CreateValidMigrationData(legacyId: legacyId);
            var migratedPublisher = PublisherTestBuilder.CreateValidPublisher(legacyId: legacyId);
            var expectedLegacyId = migratedPublisher.LegacyId;

            PublisherSourceMock
                .Setup(x => x.GetPublisherByIdOrLegacyId(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync((Publisher?)null);

            SupplierSourceMock
                .Setup(x => x.GetSupplierMigrationDataById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(rawSupplier);

            PublisherSourceMock
                .Setup(x => x.MigrateSupplier(rawSupplier, ParameterProvider.AnyToken))
                .ReturnsAsync(migratedPublisher);

            var result = await PublisherSupplierAggregator.ResolvePublisher(legacyId);

            Assert.NotNull(result);
            Assert.Equal(expectedLegacyId, result.LegacyId);
        }
    }
}