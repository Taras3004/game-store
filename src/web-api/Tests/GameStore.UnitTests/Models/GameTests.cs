using GameStore.Models.Entities;
using GameStore.UnitTests.Helpers.EntityTestBuilders;

namespace GameStore.UnitTests.Models
{
    public class GameTests
    {
        [Theory]
        [InlineData("Half-life 3", "half-life-3")]
        [InlineData("The Witcher 4", "the-witcher-4")]
        [InlineData("Counter Strike: Global Offensive", "counter-strike-global-offensive")]
        [InlineData("TEST - NAME", "test-name")]
        public void Constructor_WhenKeyIsNull_ShouldCreateProperKey(string gameName, string expectedKey)
        {
            var game = GameTestBuilder.CreateValidGame(name: gameName, key: null);

            Assert.Equal(expectedKey, game.Key);
        }

        [Fact]
        public void Constructor_WhenKeyIsProvided_ShouldNotGenerateNewKey()
        {
            string expectedKey = "new-key";

            var game = GameTestBuilder.CreateValidGame(key: expectedKey);

            Assert.Equal(expectedKey, game.Key);
        }

        [Fact]
        public void AddGenreRange_ShouldAddMultipleGenres()
        {
            var game = GameTestBuilder.CreateValidGame();
            var genres = new List<Genre>
            {
                GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid()),
                GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid())
            };

            game.AddGenreRange(genres);

            Assert.Equal(2, game.GameGenres.Count);
        }

        [Fact]
        public void AddGenreRange_NullRange_ShouldThrowArgumentNullException()
        {
            var game = GameTestBuilder.CreateValidGame();

            Assert.Throws<ArgumentNullException>(() => game.AddGenreRange(null!));
        }

        [Fact]
        public void AddPlatformRange_NullRange_ShouldThrowArgumentNullException()
        {
            var game = GameTestBuilder.CreateValidGame();

            Assert.Throws<ArgumentNullException>(() => game.AddPlatformRange(null!));
        }

        [Fact]
        public void SyncGenres_WhenAddingNewGenres_ShouldAddThemToCollection()
        {
            var game = GameTestBuilder.CreateValidGame();
            var genre1 = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());
            var genre2 = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());

            var result = game.SyncGenres([genre1, genre2]);

            Assert.True(result.IsSuccess);
            Assert.Equal(2, game.GameGenres.Count);
            Assert.Contains(game.GameGenres, gg => gg.GenreId == genre1.Id);
            Assert.Contains(game.GameGenres, gg => gg.GenreId == genre2.Id);
        }

        [Fact]
        public void SyncGenres_WhenSyncingEmptyList_ShouldRemoveAllExistingGenres()
        {
            var game = GameTestBuilder.CreateValidGame();
            var genre = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());
            
            game.SyncGenres([genre]); 
            Assert.Single(game.GameGenres);

            var result = game.SyncGenres([]);

            Assert.True(result.IsSuccess);
            Assert.Empty(game.GameGenres);
        }

        [Fact]
        public void SyncGenres_ShouldRemoveMissingAddNewAndKeepExisting()
        {
            var game = GameTestBuilder.CreateValidGame();
            
            var existingGenreToKeep = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());
            var genreToRemove = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());
            var genreToAdd = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());

            game.SyncGenres([existingGenreToKeep, genreToRemove]); 

            var result = game.SyncGenres([existingGenreToKeep, genreToAdd]);

            Assert.True(result.IsSuccess);
            Assert.Equal(2, game.GameGenres.Count);
            
            Assert.Contains(game.GameGenres, gg => gg.GenreId == existingGenreToKeep.Id);
            Assert.Contains(game.GameGenres, gg => gg.GenreId == genreToAdd.Id);
            Assert.DoesNotContain(game.GameGenres, gg => gg.GenreId == genreToRemove.Id);
        }

        [Fact]
        public void SyncGenres_WhenPassingDuplicatesInInput_ShouldNotAddDuplicates()
        {
            var game = GameTestBuilder.CreateValidGame();
            var genre = GenreTestBuilder.CreateValidGenre(id: Guid.NewGuid());

            var result = game.SyncGenres([genre, genre]);

            Assert.True(result.IsSuccess);
            Assert.Single(game.GameGenres);
        }
    }
}
