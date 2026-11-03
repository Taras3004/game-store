using GameStore.Core.Services.Aggregators;
using Moq;

namespace GameStore.UnitTests.Core.Aggregators.GenreCategory.Base
{
    public abstract class GenreCategoryAggregatorTestsBase
    {
        internal readonly Mock<IGenreSource> GenreSourceMock = new();
        internal readonly Mock<ICategorySource> CategorySourceMock = new();

        internal readonly GenreCategoryAggregator GenreCategoryAggregator;

        protected GenreCategoryAggregatorTestsBase()
        {
            GenreCategoryAggregator = new(GenreSourceMock.Object, CategorySourceMock.Object);
        }
    }
}
