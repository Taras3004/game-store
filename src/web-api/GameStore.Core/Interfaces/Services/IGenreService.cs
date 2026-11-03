using GameStore.Core.Dto.Requests.Genre;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IGenreService
    {
        Task<Result<GenreResponse>> CreateGenre(CreateGenreRequest request, CancellationToken token = default);
        Task<Result> UpdateGenre(UpdateGenreRequest request, CancellationToken token = default);
        Task<Result> DeleteGenre(Guid genreId, CancellationToken token = default);
        Task<Result<GenreResponse>> GetGenreById(Guid genreId, CancellationToken token = default);
        Task<IEnumerable<GenreResponse>> GetAllGenres(CancellationToken token = default);
        Task<IEnumerable<SubGenreResponse>> GetChildGenres(Guid parentId, CancellationToken token = default);
        Task<Result> AddParentGenre(Guid entityId, Guid parentId, CancellationToken token = default);
        Task<Result<IEnumerable<GenreResponse>>> GetGenresByGameKey(string key, CancellationToken token = default);
    }
}
