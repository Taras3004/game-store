using GameStore.Core.Dto.Responses;
using GameStore.UnitTests.Core.Aggregators.GenreCategory.Base;
using GameStore.UnitTests.Core.Helpers;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GenreCategory
{
    public class GetAllGenresAndCategoriesTests : GenreCategoryAggregatorTestsBase
    {
        [Fact]
        public async Task GetAllGenresAndCategories_FiltersOutMigratedCategories_ReturnsCombinedList()
        {
            var name1 = "Action";
            var name1Lower = "action";
            var name2 = "RPG";
            var name3 = "Strategy";

            var genres = new List<GenreResponse>
            {
                GenreTestBuilder.CreateValidGenreResponse(name: name1),
                GenreTestBuilder.CreateValidGenreResponse(name: name2),
            };

            var categories = new List<GenreResponse>
            {
                GenreTestBuilder.CreateValidGenreResponse(name: name1Lower),
                GenreTestBuilder.CreateValidGenreResponse(name: name3),
            };

            GenreSourceMock
                .Setup(x => x.GetAllGenres(ParameterProvider.AnyToken))
                .ReturnsAsync(genres);

            CategorySourceMock
                .Setup(x => x.GetAllCategoryResponses(ParameterProvider.AnyToken))
                .ReturnsAsync(categories);

            var result = await GenreCategoryAggregator.GetAllGenresAndCategories();

            var resultList = result.ToList();
            var expectedCount = 3;


            Assert.Equal(expectedCount, resultList.Count);
            Assert.Contains(resultList, x => x.Name == name1);
            Assert.Contains(resultList, x => x.Name == name2);
            Assert.Contains(resultList, x => x.Name == name3);
        }
    }
}