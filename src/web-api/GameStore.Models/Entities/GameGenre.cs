namespace GameStore.Models.Entities
{
    public class GameGenre
    {
        public Guid GameId { get; private set; }
        public Guid GenreId { get; private set; }

        public Game Game { get; private set; }
        public Genre Genre { get; private set; }

        public GameGenre(Game game, Genre genre)
        {
            Game = game;
            Genre = genre;

            GameId = game.Id;
            GenreId = genre.Id;
        }

        private GameGenre()
        {
            Game = null!;
            Genre = null!;
        }
    }
}
