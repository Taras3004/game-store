using GameStore.Core.Dto.Responses;
using GameStore.Models.Errors;
using GameStore.UnitTests.Core.Aggregators.GenreCategory.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GenreCategory
{
    public class GetGenreByIdTests : GenreCategoryAggregatorTestsBase
    {
        [Fact]
        public async Task GetGenreById_WhenIdIsGuidAndExists_ReturnsSuccessWithGenre_And_DoesNotCallNorthwind()
        {
            var guid = Guid.NewGuid();
            var setupResponse = GenreTestBuilder.CreateValidGenreResponse(id: guid.ToString());

            GenreSourceMock
                .Setup(x => x.GetGenreResponseById(guid, ParameterProvider.AnyToken))
                .ReturnsAsync(setupResponse);

            var result = await GenreCategoryAggregator.GetGenreById(guid.ToString());

            var expectedId = setupResponse.Id;

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);

            CategorySourceMock.Verify(x => x.GetCategoryResponseById(It.IsAny<string>(), ParameterProvider.AnyToken), Times.Never);
        }

        [Fact]
        public async Task GetGenreById_WhenIdIsGuidAndDoesNotExist_ReturnsFailureNotFound()
        {
            var guid = Guid.NewGuid();

            GenreSourceMock
                .Setup(x => x.GetGenreResponseById(guid, ParameterProvider.AnyToken))
                .ReturnsAsync((GenreResponse?)null);

            var result = await GenreCategoryAggregator.GetGenreById(guid.ToString());

            var expectedError = GenreErrors.NotFound;

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task GetGenreById_WhenIdIsLegacyAndDoesNotExist_ReturnsFailureNotFound()
        {
            var legacyId = "1";

            CategorySourceMock
                .Setup(x => x.GetCategoryResponseById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync((GenreResponse?)null);

            var result = await GenreCategoryAggregator.GetGenreById(legacyId);

            var expectedError = GenreErrors.NotFound;

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task GetGenreById_WhenIdIsLegacyAndAlreadyMigrated_ReturnsMigratedGenreFromGameStore()
        {
            var legacyId = "1";
            var categoryName = "Action";

            var categoryResponse = GenreTestBuilder.CreateValidGenreResponse(id: legacyId, name: categoryName);
            var migratedGenreResponse = GenreTestBuilder.CreateValidGenreResponse(name: categoryName);

            CategorySourceMock
                .Setup(x => x.GetCategoryResponseById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(categoryResponse);

            GenreSourceMock
                .Setup(x => x.GetGenreResponseByName(categoryName, ParameterProvider.AnyToken))
                .ReturnsAsync(migratedGenreResponse);

            var result = await GenreCategoryAggregator.GetGenreById(legacyId);

            var expectedId = migratedGenreResponse.Id;

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task GetGenreById_WhenIdIsLegacyAndNotMigrated_ReturnsCategoryResponse()
        {
            var legacyId = "1";
            var categoryName = "Strategy";

            var categoryResponse = GenreTestBuilder.CreateValidGenreResponse(id: legacyId, name: categoryName);

            CategorySourceMock
                .Setup(x => x.GetCategoryResponseById(legacyId, ParameterProvider.AnyToken))
                .ReturnsAsync(categoryResponse);

            GenreSourceMock
                .Setup(x => x.GetGenreResponseByName(categoryName, ParameterProvider.AnyToken))
                .ReturnsAsync((GenreResponse?)null);

            var result = await GenreCategoryAggregator.GetGenreById(legacyId);

            var expectedId = categoryResponse.Id;

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedId, result.Value.Id);
        }
    }
}