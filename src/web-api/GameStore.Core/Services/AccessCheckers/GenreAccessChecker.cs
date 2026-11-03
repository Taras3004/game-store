using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class GenreAccessChecker(IGenreService genreService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await genreService.GetGenreById(targetId, token);

            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
