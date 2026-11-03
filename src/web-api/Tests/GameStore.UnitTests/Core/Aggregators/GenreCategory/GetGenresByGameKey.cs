using GameStore.Core.Dto.Responses;
using GameStore.Models.Errors;
using GameStore.Models.Results;
using GameStore.UnitTests.Core.Aggregators.GenreCategory.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GenreCategory
{
    public class GetGenresByGameKeyTests : GenreCategoryAggregatorTestsBase
    {
        [Fact]
        public async Task GetGenresByGameKey_WhenCategorySourceFails_ReturnsFailure()
        {
            var gameKey = "game-1";
            var mockGenres = new List<GenreResponse>();
            var mockLegacyIds = new HashSet<string>();
            var setupError = GameErrors.NotFound;

            GenreSourceMock
                .Setup(x => x.GetGenresByGameKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync((mockGenres, mockLegacyIds));

            CategorySourceMock
                .Setup(x => x.GetCategoryResponsesByProductKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Failure<IEnumerable<GenreResponse>>(setupError));

            var result = await GenreCategoryAggregator.GetGenresByGameKey(gameKey);

            var expectedError = setupError;

            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task GetGenresByGameKey_SuccessfullyCombinesGenresAndUnmigratedCategories()
        {
            var gameKey = "game-1";
            var name1 = "RPG";
            var name2 = "Action";
            var name3 = "Strategy";

            var genre1 = GenreTestBuilder.CreateValidGenreResponse(name: name1);
            var genre2 = GenreTestBuilder.CreateValidGenreResponse(name: name2);

            var genresList = new List<GenreResponse> { genre1, genre2 };
            var legacyIdsSet = new HashSet<string> { "1" };

            GenreSourceMock
                .Setup(x => x.GetGenresByGameKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync((genresList, legacyIdsSet));

            var category1 = GenreTestBuilder.CreateValidGenreResponse(id: "1", name: name2);
            var category2 = GenreTestBuilder.CreateValidGenreResponse(id: "2", name: name3);
            var categoriesList = new List<GenreResponse> { category1, category2 };

            CategorySourceMock
                .Setup(x => x.GetCategoryResponsesByProductKey(gameKey, ParameterProvider.AnyToken))
                .ReturnsAsync(Result.Success<IEnumerable<GenreResponse>>(categoriesList));

            var result = await GenreCategoryAggregator.GetGenresByGameKey(gameKey);

            var resultList = result.Value.ToList();

            Assert.True(result.IsSuccess);
            Assert.Equal(3, resultList.Count);
            Assert.Contains(resultList, x => x.Name == name1);
            Assert.Contains(resultList, x => x.Name == name2);
            Assert.Contains(resultList, x => x.Name == name3);
        }
    }
}