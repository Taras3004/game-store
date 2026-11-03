using GameStore.Core.Dto.Requests.Platform;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IPlatformService
    {
        Task<Result<PlatformResponse>> CreatePlatform(CreatePlatformRequest request, CancellationToken token = default);
        Task<Result> UpdatePlatform(UpdatePlatformRequest request, CancellationToken token = default);
        Task<Result> DeletePlatform(Guid platformId, CancellationToken token = default);
        Task<Result<PlatformResponse>> GetPlatform(Guid platformId, CancellationToken token = default);
        Task<IEnumerable<PlatformResponse>> GetAllPlatforms(CancellationToken token = default);
        Task<IEnumerable<PlatformResponse>> GetPlatformsByGameKey(string gameKey, CancellationToken token = default);
    }
}
