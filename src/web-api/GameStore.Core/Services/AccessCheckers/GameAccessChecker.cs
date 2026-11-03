using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class GameAccessChecker(IGameService gameService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await gameService.GetGameById(targetId, token);

            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
