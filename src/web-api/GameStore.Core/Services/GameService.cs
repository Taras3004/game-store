using AutoMapper;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Dto.Requests.Game;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Sorting;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.DomainEvents.Events.Games;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Errors;
using GameStore.Models.Helpers;
using GameStore.Models.Results;
using GameStore.Models.ValueObjects;
using System.Text;
using System.Text.Json;

namespace GameStore.Core.Services
{
    internal class GameService(
        IUnitOfWork unitOfWork,
        IPlatformInternalService platformInternalService,
        IUserProvider userProvider,
        IMapper mapper) : IGameService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        public async Task<Result<GameResponse>> CreateGame(
            CreateGameRequest request, CancellationToken token = default)
        {
            string gameKey = string.IsNullOrWhiteSpace(request.Game.Key)
                ? KeyGenerator.GenerateKey(request.Game.Name)
                : request.Game.Key;

            if (await unitOfWork.Games.IsKeyTakenAsync(gameKey, excludeId: null, token))
            {
                return Result.Failure<GameResponse>(GameErrors.KeyAlreadyTaken);
            }

            var gamePriceResult = GamePrice.Create(request.Game.Price, request.Game.Discount);
            if (gamePriceResult.IsFailure)
            {
                return Result.Failure<GameResponse>(gamePriceResult.Error);
            }

            var publisher = await unitOfWork.Publishers.GetOrMigrateByIdAsync(request.Publisher, token);
            if (publisher is null)
            {
                return Result.Failure<GameResponse>(PublisherErrors.NotFound);
            }

            var game = new Game(
                request.Game.Name,
                gameKey,
                request.Game.Description,
                gamePriceResult.Value,
                request.Game.UnitInStock,
                publisher.Id);

            if (request.Genres?.Any() == true)
            {
                var genres = await unitOfWork.Genres.GetOrMigrateByIdsAsync(request.Genres, token);
                game.AddGenreRange(genres);
            }

            if (request.Platforms?.Any() == true)
            {
                var platforms = await platformInternalService.GetPlatformsByIds(request.Platforms, token);
                game.AddPlatformRange(platforms);
            }

            await unitOfWork.Games.AddAsync(game, token);
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success(mapper.Map<GameResponse>(game));
        }

        public async Task<Result> UpdateGame(
            UpdateGameRequest request, CancellationToken token = default)
        {
            Guid gameId = request.Game.Id;

            string expectedKey = string.IsNullOrWhiteSpace(request.Game.Key)
                ? KeyGenerator.GenerateKey(request.Game.Name)
                : request.Game.Key;

            if (await unitOfWork.Games.IsKeyTakenAsync(expectedKey, excludeId: gameId, token))
            {
                return Result.Failure(GameErrors.KeyAlreadyTaken);
            }

            var gameToUpdate = await unitOfWork.Games.GetByIdAsync(gameId, GetViewPermissionDetails(), TrackingBehaviour.Tracking, token);
            if (gameToUpdate is null)
            {
                return Result.Failure(GameErrors.NotFound);
            }

            if (gameToUpdate.IsDeleted && !userProvider.HasPermission(Permission.UpdateDeletedGame))
            {
                return Result.Failure(GameErrors.ModificationForbidden);
            }

            var gamePriceResult = GamePrice.Create(request.Game.Price, request.Game.Discount);
            if (gamePriceResult.IsFailure)
            {
                return Result.Failure(gamePriceResult.Error);
            }

            var publisher = await unitOfWork.Publishers.GetOrMigrateByIdAsync(request.Publisher, token);
            if (publisher is null)
            {
                return Result.Failure(PublisherErrors.NotFound);
            }

            if (gameToUpdate.Key != expectedKey)
            {
                gameToUpdate.Raise(new GameKeyUpdatedEvent(gameToUpdate.Id, gameToUpdate.Key, expectedKey));
            }

            gameToUpdate.UpdateDetails(
                request.Game.Name,
                expectedKey,
                request.Game.Description,
                gamePriceResult.Value,
                request.Game.UnitInStock,
                publisher.Id);

            if (request.Genres is not null)
            {
                var genres = await unitOfWork.Genres.GetOrMigrateByIdsAsync(request.Genres, token);
                var syncGenresResult = gameToUpdate.SyncGenres(genres);
                if (syncGenresResult.IsFailure)
                {
                    return Result.Failure(syncGenresResult.Error);
                }
            }

            if (request.Platforms is not null)
            {
                var platforms = await platformInternalService.GetPlatformsByIds(request.Platforms, token);
                var syncPlatformsResult = gameToUpdate.SyncPlatforms(platforms);
                if (syncPlatformsResult.IsFailure)
                {
                    return Result.Failure(syncPlatformsResult.Error);
                }
            }

            await unitOfWork.Games.UpdateAsync(gameToUpdate, token);
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result> DeleteGame(string key, CancellationToken token = default)
        {
            var game = await unitOfWork.Games.GetByKeyAsync(key, GetViewPermissionDetails(), TrackingBehaviour.Tracking, token);
            if (game is null)
            {
                return Result.Success();
            }

            if (!string.IsNullOrEmpty(game.LegacyId))
            {
                return Result.Failure(GameErrors.CannotDeleteLegacyData);
            }

            if (game.IsDeleted)
            {
                return Result.Success();
            }

            game.Delete();
            await unitOfWork.Games.UpdateAsync(game, token);
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result<GameResponse>> GetGameByKey(string key, CancellationToken token = default)
        {
            var game = await unitOfWork.Games.GetByKeyAsync(key, GetViewPermissionDetails(), token: token);
            if (game is null)
            {
                return Result.Failure<GameResponse>(GameErrors.NotFound);
            }

            return Result.Success(mapper.Map<GameResponse>(game));
        }

        public async Task<Result<GameResponse>> GetGameById(Guid gameId, CancellationToken token = default)
        {
            var game = await unitOfWork.Games.GetByIdAsync(gameId, GetViewPermissionDetails(), token: token);
            if (game is null)
            {
                return Result.Failure<GameResponse>(GameErrors.NotFound);
            }

            return Result.Success(mapper.Map<GameResponse>(game));
        }

        public async Task<Result<GameResponse>> GetGameByIdAndIncrementViews(
            Guid gameId, CancellationToken token = default)
        {
            var getResult = await GetGameById(gameId, token);
            if (getResult.IsFailure)
            {
                return Result.Failure<GameResponse>(getResult.Error);
            }

            await unitOfWork.Games.IncrementViewsByIdAsync(gameId, token);
            return getResult.Value;
        }

        public async Task<Result<GameResponse>> GetGameByKeyAndIncrementViews(
            string gameKey, CancellationToken token = default)
        {
            var getResult = await GetGameByKey(gameKey, token);
            if (getResult.IsFailure)
            {
                return Result.Failure<GameResponse>(getResult.Error);
            }

            await unitOfWork.Games.IncrementViewsByKeyAsync(gameKey, token);
            return getResult.Value;
        }

        public async Task<Result<GamesPage>> GetAllGames(
            GameFilterDetails gameFilterDetails, CancellationToken token = default)
        {
            if (!string.IsNullOrWhiteSpace(gameFilterDetails.Name) && gameFilterDetails.Name.Length < GameValidation.MinNameFilterChars)
            {
                return Result.Failure<GamesPage>(GameErrors.NameFilterTooSmall);
            }

            var games = await unitOfWork.Games.GetByFilterAsync(gameFilterDetails, GetViewPermissionDetails(), token);
            var mappedGames = mapper.Map<List<GameResponse>>(games);

            mappedGames = ApplySorting(mappedGames, gameFilterDetails.Sort);

            int totalItems = mappedGames.Count;
            int currentPage = gameFilterDetails.Page is null || gameFilterDetails.Page < 1 ? 1 : gameFilterDetails.Page.Value;
            int? pageSize = null;

            if (!string.IsNullOrWhiteSpace(gameFilterDetails.PageCount) &&
                FilteringMappings.PaginationMap.TryGetValue(gameFilterDetails.PageCount, out int parsedSize))
            {
                pageSize = parsedSize;
                int skip = parsedSize * (currentPage - 1);
                mappedGames = mappedGames.Skip(skip).Take(parsedSize).ToList();
            }

            int totalPages = pageSize is null || pageSize == 0
                ? 1
                : (int)Math.Ceiling(totalItems / (double)pageSize.Value);

            return Result.Success(new GamesPage
            {
                Games = mappedGames,
                CurrentPage = currentPage,
                TotalPages = totalPages
            });
        }

        public async Task<Result<GameFileResponse>> DownloadGameFile(string key, CancellationToken token = default)
        {
            var gameResult = await GetGameByKey(key, token);
            if (gameResult.IsFailure)
            {
                return Result.Failure<GameFileResponse>(GameErrors.NotFound);
            }

            var gameResponse = gameResult.Value;
            string serializedData = JsonSerializer.Serialize(gameResponse, _jsonSerializerOptions);
            byte[] fileBytes = Encoding.UTF8.GetBytes(serializedData);

            return new GameFileResponse
            {
                FileContent = fileBytes,
                ContentType = "text/plain",
                FileName = $"_{gameResponse.Key}.txt"
            };
        }

        public async Task<int> GetGameCount(
            GameFilterDetails? filter, CancellationToken token = default)
        {
            return await unitOfWork.Games.GetCountAsync(
                filter, GetViewPermissionDetails(), token);
        }

        public async Task<IEnumerable<GameResponse>> GetGamesByGenreId(Guid genreId, CancellationToken token = default)
        {
            var games = await unitOfWork.Games.GetByGenreIdAsync(genreId, GetViewPermissionDetails(), token);
            return mapper.Map<IEnumerable<GameResponse>>(games);
        }

        public async Task<IEnumerable<GameResponse>> GetGamesByPlatformId(Guid platformId, CancellationToken token = default)
        {
            var games = await unitOfWork.Games.GetByPlatformIdAsync(platformId, GetViewPermissionDetails(), token);
            return mapper.Map<IEnumerable<GameResponse>>(games);
        }

        public async Task<IEnumerable<GameResponse>> GetGamesByPublisherCompanyName(
            string companyName, CancellationToken token = default)
        {
            var games = await unitOfWork.Games.GetByPublisherCompanyNameAsync(
                companyName, GetViewPermissionDetails(), token);
            return mapper.Map<IEnumerable<GameResponse>>(games);
        }

        public IEnumerable<string> GetSortingOptions() => SortingMappings.GameSortingMap.Keys;
        public IEnumerable<string> GetPaginationOptions() => FilteringMappings.PaginationMap.Keys;
        public IEnumerable<string> GetDateFilteringOptions() => FilteringMappings.DateFilteringMap.Keys;

        private static List<GameResponse> ApplySorting(List<GameResponse> games, string? sortOption)
        {
            if (string.IsNullOrWhiteSpace(sortOption) ||
                !SortingMappings.GameSortingMap.TryGetValue(sortOption, out GameSortOrder order))
            {
                return games;
            }

            return order switch
            {
                GameSortOrder.PriceAsc => games.OrderBy(g => g.Price).ToList(),
                GameSortOrder.PriceDesc => games.OrderByDescending(g => g.Price).ToList(),
                _ => games
            };
        }

        private GamePermissionDetails GetViewPermissionDetails()
        {
            if (!userProvider.IsAuthenticated)
            {
                return new GamePermissionDetails
                {
                    AccessDeleted = false,
                    AccessOutOfStock = false
                };
            }

            return new GamePermissionDetails
            {
                AccessDeleted = userProvider.HasPermission(Permission.ViewDeletedGame),
                AccessOutOfStock = userProvider.HasPermission(Permission.ViewOutOfStockGames)
            };
        }


    }
}