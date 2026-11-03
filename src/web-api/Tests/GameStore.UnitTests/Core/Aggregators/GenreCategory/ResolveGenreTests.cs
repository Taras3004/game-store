using GameStore.Core.Dto.Details.MigrationData;
using GameStore.Models.Entities;
using GameStore.UnitTests.Core.Aggregators.GenreCategory.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GenreCategory
{
    public class ResolveGenreTests : GenreCategoryAggregatorTestsBase
    {
        [Fact]
        public async Task ResolveGenre_WhenFoundInGameStore_ReturnsGenre_And_DoesNotCallNorthwind()
        {
            var id = Guid.NewGuid().ToString();
            var existingGenre = GenreTestBuilder.CreateValidGenre(id: Guid.Parse(id));

            GenreSourceMock
                .Setup(x => x.GetGenreByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync(existingGenre);

            var result = await GenreCategoryAggregator.ResolveGenre(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(existingGenre.Id, result.Id);
        }

        [Fact]
        public async Task ResolveGenre_WhenNotFoundInBothSources_ReturnsNull()
        {
            var id = "someId";

            GenreSourceMock
                .Setup(x => x.GetGenreByIdOrLegacyId(id, ParameterProvider.AnyToken))
                .ReturnsAsync((Genre?)null);

            CategorySourceMock
                .Setup(x => x.GetCategoryMigrationDataById(id, ParameterProvider.AnyToken))
                .ReturnsAsync((CategoryMigrationData?)null);

            var result = await GenreCategoryAggregator.ResolveGenre(id, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task ResolveGenre_WhenFoundOnlyInNorthwind_MigratesAndReturnsGenre()
        {
            var legacyId = "1";

            GenreSourceMock
                .Setup(x => x.GetGenreByIdOrLegacyId(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync((Genre?)null);

            var rawCategory = GenreTestBuilder.CreateValidMigrationData(legacyId: legacyId);
            var migratedGenre = GenreTestBuilder.CreateValidGenre(legacyId: legacyId);

            CategorySourceMock
                .Setup(x => x.GetCategoryMigrationDataById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(rawCategory);

            GenreSourceMock
                .Setup(x => x.MigrateCategory(rawCategory, ParameterProvider.AnyToken))
                .ReturnsAsync(migratedGenre);

            var result = await GenreCategoryAggregator.ResolveGenre(legacyId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(legacyId, result.LegacyId);
        }
    }
}