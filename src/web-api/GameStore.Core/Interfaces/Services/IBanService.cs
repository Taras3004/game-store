using GameStore.Core.Dto.Requests.Ban;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IBanService
    {
        Task<Result> BanUser(BanUserRequest request, CancellationToken token = default);
        Task<bool> IsUserBanned(string username, CancellationToken token = default);
        IEnumerable<string> GetBanDurations();
    }
}