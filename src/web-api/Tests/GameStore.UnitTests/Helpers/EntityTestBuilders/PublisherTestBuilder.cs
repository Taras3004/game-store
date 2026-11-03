using GameStore.Core.Dto.Details.MigrationData;
using GameStore.Core.Dto.Details.Publisher;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Entities;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class PublisherTestBuilder
    {
        public static Publisher CreateValidPublisher(
            string companyName = EntityBuilderConstants.ProvidedByBuilderString,
            string? homePage = EntityBuilderConstants.ProvidedByBuilderString,
            string? description = EntityBuilderConstants.ProvidedByBuilderString,
            string? legacyId = EntityBuilderConstants.ProvidedByBuilderString,
            Guid id = default)
        {
            return new Publisher(
                companyName,
                homePage,
                description,
                id: id)
            {
                LegacyId = legacyId
            };
        }

        public static PublisherResponse CreateValidPublisherResponse(
            string companyName = EntityBuilderConstants.ProvidedByBuilderString,
            string? homePage = EntityBuilderConstants.ProvidedByBuilderString,
            string? description = EntityBuilderConstants.ProvidedByBuilderString,
            string id = EntityBuilderConstants.ProvidedByBuilderString)
        {
            return new PublisherResponse
            {
                CompanyName = companyName,
                HomePage = homePage,
                Description = description,
                Id = id
            };
        }

        public static SupplierMigrationData CreateValidMigrationData(
            string legacyId = EntityBuilderConstants.ProvidedByBuilderString,
            string companyName = EntityBuilderConstants.ProvidedByBuilderString,
            string? homePage = EntityBuilderConstants.ProvidedByBuilderString)
        {
            return new SupplierMigrationData
            {
                LegacyId = legacyId,
                PublisherDetails = new PublisherDetailsBase
                {
                    CompanyName = companyName,
                    HomePage = homePage
                }
            };
        }
    }
}