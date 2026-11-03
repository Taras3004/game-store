using GameStore.Core.Dto.Details.MigrationData;
using GameStore.Models.Entities;
using GameStore.UnitTests.Core.Aggregators.GenreCategory.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GenreCategory
{
    public class ResolveGenresTests : GenreCategoryAggregatorTestsBase
    {
        [Fact]
        public async Task ResolveGenres_WhenOnlyGuidsProvided_ReturnsExistingGenres_And_DoesNotMigrate()
        {
            var guidId = Guid.NewGuid().ToString();
            var inputIds = new List<string> { guidId };

            var existingGenre = GenreTestBuilder.CreateValidGenre(id: Guid.Parse(guidId));
            var existingGenresList = new List<Genre> { existingGenre };

            GenreSourceMock
                .Setup(x => x.GetGenresByLegacyIdsOrIds(inputIds, ParameterProvider.AnyToken))
                .ReturnsAsync(existingGenresList);

            var result = await GenreCategoryAggregator.ResolveGenres(inputIds, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(existingGenre.Id, result[0].Id);
        }

        [Fact]
        public async Task ResolveGenres_WhenLegacyIdsAlreadyMigrated_ReturnsExisting_And_DoesNotMigrate()
        {
            var legacyId = "1";
            var inputIds = new List<string> { legacyId };

            var migratedGenre = GenreTestBuilder.CreateValidGenre(legacyId: legacyId);
            var existingGenresList = new List<Genre> { migratedGenre };

            GenreSourceMock
                .Setup(x => x.GetGenresByLegacyIdsOrIds(inputIds, ParameterProvider.AnyToken))
                .ReturnsAsync(existingGenresList);

            var result = await GenreCategoryAggregator.ResolveGenres(inputIds, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(legacyId, result[0].LegacyId);
        }

        [Fact]
        public async Task ResolveGenres_WhenUnmappedLegacyIdsExist_MigratesAndReturnsCombinedList()
        {
            var legacyId = "1";
            var inputIds = new List<string> { legacyId };

            GenreSourceMock
                .Setup(x => x.GetGenresByLegacyIdsOrIds(inputIds, ParameterProvider.AnyToken))
                .ReturnsAsync([]);

            var rawCategories = new List<CategoryMigrationData>
            {
                GenreTestBuilder.CreateValidMigrationData(legacyId: legacyId)
            };

            var migratedGenres = new List<Genre>
            {
                GenreTestBuilder.CreateValidGenre(legacyId: legacyId)
            };

            CategorySourceMock
                .Setup(x => x.GetMigrationDataByIds(It.Is<IEnumerable<string>>(ids => ids.Contains(legacyId)), ParameterProvider.AnyToken))
                .ReturnsAsync(rawCategories);

            GenreSourceMock
                .Setup(x => x.MigrateCategories(rawCategories, ParameterProvider.AnyToken))
                .ReturnsAsync(migratedGenres);

            var result = await GenreCategoryAggregator.ResolveGenres(inputIds, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(legacyId, result[0].LegacyId);
        }

        [Fact]
        public async Task ResolveGenres_WithMixedInputIds_CorrectlyFiltersAndMigratesOnlyUnmapped()
        {
            var guidId = Guid.NewGuid().ToString();
            var migratedLegacyId = "1";
            var unmigratedLegacyId = "2";

            var inputIds = new List<string> { guidId, migratedLegacyId, unmigratedLegacyId };

            var existingGenre1 = GenreTestBuilder.CreateValidGenre(id: Guid.Parse(guidId));
            var existingGenre2 = GenreTestBuilder.CreateValidGenre(legacyId: migratedLegacyId);

            GenreSourceMock
                .Setup(x => x.GetGenresByLegacyIdsOrIds(inputIds, ParameterProvider.AnyToken))
                .ReturnsAsync(new List<Genre> { existingGenre1, existingGenre2 });

            var rawCategories = new List<CategoryMigrationData>
            {
                GenreTestBuilder.CreateValidMigrationData(legacyId: unmigratedLegacyId)
            };

            var newMigratedGenre = GenreTestBuilder.CreateValidGenre(legacyId: unmigratedLegacyId);

            CategorySourceMock
                .Setup(x => x.GetMigrationDataByIds(It.Is<IEnumerable<string>>(ids => ids.Count() == 1 && ids.Contains(unmigratedLegacyId)), ParameterProvider.AnyToken))
                .ReturnsAsync(rawCategories);

            GenreSourceMock
                .Setup(x => x.MigrateCategories(rawCategories, ParameterProvider.AnyToken))
                .ReturnsAsync(new List<Genre> { newMigratedGenre });

            var result = await GenreCategoryAggregator.ResolveGenres(inputIds, CancellationToken.None);

            Assert.Equal(3, result.Count);
            Assert.Contains(result, g => g.Id == existingGenre1.Id);
            Assert.Contains(result, g => g.LegacyId == migratedLegacyId);
            Assert.Contains(result, g => g.LegacyId == unmigratedLegacyId);
        }
    }
}
