using GameStore.UnitTests.Helpers.EntityTestBuilders;

namespace GameStore.UnitTests.Models
{
    public class GenreTests
    {
        [Fact]
        public void SetParent_WhenParentIsValid_ShouldSetParentAndId()
        {
            var parentId = Guid.NewGuid();
            var parentGenre = GenreTestBuilder.CreateValidGenre(id: parentId);
            var childGenre = GenreTestBuilder.CreateValidGenre();

            childGenre.SetParent(parentGenre);

            Assert.Equal(parentGenre, childGenre.ParentGenre);
            Assert.Equal(parentId, childGenre.ParentGenreId);
        }

        [Fact]
        public void SetParent_WhenParentIsNull_ShouldThrowArgumentNullException()
        {
            var genre = GenreTestBuilder.CreateValidGenre();

            Assert.Throws<ArgumentNullException>(() => genre.SetParent(null!));
        }
    }
}
