using GameStore.Core.Dto.Details.Genre;
using GameStore.Core.Dto.Details.MigrationData;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Entities;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class GenreTestBuilder
    {
        public static Genre CreateValidGenre(
                    string name = EntityBuilderConstants.ProvidedByBuilderString,
                    Guid? parentGenreId = null,
                    string legacyId = EntityBuilderConstants.ProvidedByBuilderString,
                    Guid id = default)
        {
            return new Genre(
                name,
                parentGenreId,
                id)
            {
                LegacyId = legacyId
            };
        }

        public static GenreDetailsBase CreateValidGenreDetailsBase(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            Guid? parentGenreId = null)
        {
            return new GenreDetailsBase
            {
                Name = name,
                ParentGenreId = parentGenreId
            };
        }

        public static GenreDetailsWithId CreateValidGenreDetailsWithId(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            Guid? parentGenreId = null,
            Guid id = default)
        {
            return new GenreDetailsWithId
            {
                Name = name,
                ParentGenreId = parentGenreId,
                Id = id.ToString()
            };
        }

        public static GenreResponse CreateValidGenreResponse(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            Guid? parentGenreId = null,
            string id = EntityBuilderConstants.ProvidedByBuilderString
        )
        {
            return new GenreResponse
            {
                Name = name,
                ParentGenreId = parentGenreId.ToString(),
                Id = id
            };
        }

        public static CategoryMigrationData CreateValidMigrationData(
            string legacyId = EntityBuilderConstants.ProvidedByBuilderString,
            string name = EntityBuilderConstants.ProvidedByBuilderString
        )
        {
            return new CategoryMigrationData()
            {
                LegacyId = legacyId,
                Name = name
            };
        }
    }
}