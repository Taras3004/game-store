using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;
using GameStore.Models.Errors;
using GameStore.Models.Helpers;
using GameStore.Models.Results;
using GameStore.Models.ValueObjects;

namespace GameStore.Models.Entities
{
    public class Game : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Key { get; private set; }
        public string? Description { get; private set; }
        public int UnitInStock { get; private set; }
        public GamePrice GamePrice { get; private set; }
        public Guid PublisherId { get; private set; }
        public Publisher Publisher { get; private set; }

        public int Views { get; private set; }
        public DateTime? PublishingDate { get; private set; }
        public int CommentsCount { get; private set; }

        public IReadOnlyCollection<GameGenre> GameGenres => gameGenres;
        private readonly List<GameGenre> gameGenres = [];

        public IReadOnlyCollection<GamePlatform> GamePlatforms => gamePlatforms;
        private readonly List<GamePlatform> gamePlatforms = [];

        public string? QuantityPerUnit { get; init; }
        public int? UnitsOnOrder { get; init; }
        public int? ReorderLevel { get; init; }
        public string? LegacyId { get; init; }

        public bool IsDeleted { get; private set; }

        public Game(string name, string? key, string? description, GamePrice gamePrice, int unitInStock, Guid publisherId, Guid id = default, int views = 0)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name;
            Key = string.IsNullOrEmpty(key) ? KeyGenerator.GenerateKey(Name) : key;
            Description = description;
            GamePrice = gamePrice;
            UnitInStock = unitInStock;
            PublisherId = publisherId;
            Publisher = null!;
            PublishingDate = DateTime.UtcNow;
            CommentsCount = 0;
            Views = views;
        }

        private Game()
        {
            Name = null!;
            Key = null!;
            GamePrice = null!;
            Publisher = null!;
        }

        public void AddGenreRange(IEnumerable<Genre> range)
        {
            ArgumentNullException.ThrowIfNull(range);

            foreach (var genre in range)
            {
                AddGenre(genre);
            }
        }

        public void AddGenre(Genre genre)
        {
            ArgumentNullException.ThrowIfNull(genre);

            if (gameGenres.Any(x => x.GenreId == genre.Id))
            {
                return;
            }
            gameGenres.Add(new GameGenre(this, genre));
        }

        private Result RemoveGenre(Guid id)
        {
            var gameGenre = gameGenres.FirstOrDefault(x => x.GenreId == id);

            if (gameGenre == null)
            {
                return Result.Failure(GameErrors.RelatedGenreNotFound);
            }

            gameGenres.Remove(gameGenre);

            return Result.Success();
        }

        public void AddPlatformRange(IEnumerable<Platform> range)
        {
            ArgumentNullException.ThrowIfNull(range);

            foreach (var platform in range)
            {
                AddPlatform(platform);
            }
        }

        private void AddPlatform(Platform platform)
        {
            ArgumentNullException.ThrowIfNull(platform);

            if (gamePlatforms.Any(x => x.PlatformId == platform.Id))
            {
                return;
            }
            gamePlatforms.Add(new GamePlatform(this, platform));
        }

        private Result RemovePlatform(Guid id)
        {
            var gamePlatform = gamePlatforms.FirstOrDefault(x => x.PlatformId == id);

            if (gamePlatform == null)
            {
                return Result.Failure(GameErrors.RelatedPlatformNotFound);
            }

            gamePlatforms.Remove(gamePlatform);

            return Result.Success();
        }

        public Result SyncPlatforms(IEnumerable<Platform> newPlatforms)
        {
            ArgumentNullException.ThrowIfNull(newPlatforms);

            IEnumerable<Platform> platforms = newPlatforms.ToArray();
            var newPlatformIds = platforms.Select(x => x.Id).ToList();
            var trackedPlatformIds = gamePlatforms.Select(x => x.PlatformId).ToList();

            var idsToRemove = trackedPlatformIds.Except(newPlatformIds).ToList();
            foreach (var id in idsToRemove)
            {
                var result = RemovePlatform(id);
                if (result.IsFailure)
                {
                    return result;
                }
            }

            var platformsToAdd = platforms.Where(p => !trackedPlatformIds.Contains(p.Id)).ToList();
            foreach (var platform in platformsToAdd)
            {
                AddPlatform(platform);
            }

            return Result.Success();
        }

        public Result SyncGenres(IEnumerable<Genre> newGenres)
        {
            ArgumentNullException.ThrowIfNull(newGenres);

            IEnumerable<Genre> genres = newGenres.ToArray();
            var newGenresIds = genres.Select(x => x.Id).ToList();
            var trackedGenresIds = gameGenres.Select(x => x.GenreId).ToList();

            var idsToRemove = trackedGenresIds.Except(newGenresIds).ToList();
            foreach (var id in idsToRemove)
            {
                var result = RemoveGenre(id);
                if (result.IsFailure)
                {
                    return result;
                }
            }

            var genresToAdd = genres.Where(p => !trackedGenresIds.Contains(p.Id)).ToList();
            foreach (var genre in genresToAdd)
            {
                AddGenre(genre);
            }

            return Result.Success();
        }

        public void UpdateDetails(string name, string? key, string? description, GamePrice gamePrice, int unitInStock, Guid publisherId)
        {
            Name = name;
            Key = key ?? KeyGenerator.GenerateKey(Name);
            Description = description;
            GamePrice = gamePrice;
            UnitInStock = unitInStock;
            PublisherId = publisherId;
        }

        public Result ReduceUnitsInStock(int amount)
        {
            if (amount > UnitInStock)
            {
                return Result.Failure(GameErrors.OutOfStock);
            }

            UnitInStock -= amount;
            return Result.Success();
        }

        public void AddUnitsInStock(int amount)
        {
            UnitInStock += amount;
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public List<IDomainEvent> DomainEvents => domainEvents;
        private readonly List<IDomainEvent> domainEvents = new();

        public void Raise(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }
    }
}
