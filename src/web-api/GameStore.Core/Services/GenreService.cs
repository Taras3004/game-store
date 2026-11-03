using AutoMapper;
using GameStore.Core.Dto.Requests.Genre;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class GenreService(
        IGenreRepository genreRepository,
        IUnitOfWork gameStoreUnitOfWork,
        IMapper mapper) : IGenreService
    {
        public async Task<Result<GenreResponse>> CreateGenre(
            CreateGenreRequest request, CancellationToken token = default)
        {
            if (await genreRepository.IsNameTakenAsync(request.Genre.Name, excludeId: null, token))
            {
                return Result.Failure<GenreResponse>(GenreErrors.AlreadyExists);
            }

            if (request.Genre.ParentGenreId.HasValue)
            {
                var parent = await genreRepository.GetByIdAsync(request.Genre.ParentGenreId.Value, token: token);
                if (parent is null)
                {
                    return Result.Failure<GenreResponse>(GenreErrors.ParentNotFound);
                }
            }

            var genre = new Genre(request.Genre.Name, request.Genre.ParentGenreId);
            await genreRepository.AddAsync(genre, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success(mapper.Map<GenreResponse>(genre));
        }

        public async Task<Result> UpdateGenre(
            UpdateGenreRequest request, CancellationToken token = default)
        {
            Guid genreId = request.Genre.Id;

            if (request.Genre.ParentGenreId == genreId)
            {
                return Result.Failure(GenreErrors.ParentItselfConflict);
            }

            if (await genreRepository.IsNameTakenAsync(request.Genre.Name, genreId, token))
            {
                return Result.Failure(GenreErrors.NameConflict);
            }

            var genreToUpdate = await genreRepository.GetByIdAsync(genreId, TrackingBehaviour.Tracking, token);
            if (genreToUpdate is null)
            {
                return Result.Failure(GenreErrors.NotFound);
            }

            if (request.Genre.ParentGenreId.HasValue)
            {
                var parent = await genreRepository.GetByIdAsync(request.Genre.ParentGenreId.Value, token: token);
                if (parent is null)
                {
                    return Result.Failure(GenreErrors.ParentNotFound);
                }
            }

            genreToUpdate.UpdateDetails(request.Genre.Name, request.Genre.ParentGenreId);
            await genreRepository.UpdateAsync(genreToUpdate, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result> DeleteGenre(Guid genreId, CancellationToken token = default)
        {
            var genre = await genreRepository.GetByIdAsync(genreId, TrackingBehaviour.Tracking, token);
            if (genre is null)
            {
                return Result.Failure(GenreErrors.NotFound);
            }

            if (!string.IsNullOrEmpty(genre.LegacyId))
            {
                return Result.Failure(GenreErrors.CannotDeleteLegacyData);
            }

            await genreRepository.DeleteAsync(genre, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result<GenreResponse>> GetGenreById(Guid genreId, CancellationToken token = default)
        {
            var genre = await genreRepository.GetByIdAsync(genreId, token: token);
            if (genre is null)
            {
                return Result.Failure<GenreResponse>(GenreErrors.NotFound);
            }

            return Result.Success(mapper.Map<GenreResponse>(genre));
        }

        public async Task<IEnumerable<GenreResponse>> GetAllGenres(CancellationToken token = default)
        {
            var genres = await genreRepository.GetAllAsync(token);
            return mapper.Map<IEnumerable<GenreResponse>>(genres);
        }

        public async Task<IEnumerable<SubGenreResponse>> GetChildGenres(Guid parentId, CancellationToken token = default)
        {
            var childGenres = await genreRepository.GetByParentIdAsync(parentId, token);
            return mapper.Map<IEnumerable<SubGenreResponse>>(childGenres);
        }

        public async Task<Result> AddParentGenre(Guid entityId, Guid parentId, CancellationToken token = default)
        {
            if (entityId == parentId)
            {
                return Result.Failure(GenreErrors.ParentItselfConflict);
            }

            var entity = await genreRepository.GetByIdAsync(entityId, TrackingBehaviour.Tracking, token);
            if (entity is null)
            {
                return Result.Failure(GenreErrors.NotFound);
            }

            // The parent becomes a navigation of a SQL entity, so it must be the tracked SQL instance
            var parentEntity = (await genreRepository.GetOrMigrateByIdsAsync([parentId], token)).FirstOrDefault();
            if (parentEntity is null)
            {
                return Result.Failure(GenreErrors.ParentNotFound);
            }

            entity.SetParent(parentEntity);
            await genreRepository.UpdateAsync(entity, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<GenreResponse>>> GetGenresByGameKey(
            string key, CancellationToken token = default)
        {
            var genres = await genreRepository.GetByGameKeyAsync(key, token);
            return Result.Success(mapper.Map<IEnumerable<GenreResponse>>(genres));
        }
    }
}