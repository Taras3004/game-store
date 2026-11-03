namespace GameStore.Models.Entities
{
    public class GamePlatform
    {
        public Guid GameId { get; private set; }
        public Guid PlatformId { get; private set; }

        public Game Game { get; private set; }
        public Platform Platform { get; private set; }

        public GamePlatform(Game game, Platform platform)
        {
            Game = game;
            Platform = platform;

            GameId = game.Id;
            PlatformId = platform.Id;
        }

        private GamePlatform()
        {
            Game = null!;
            Platform = null!;
        }
    }
}
