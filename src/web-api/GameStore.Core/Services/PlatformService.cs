using AutoMapper;
using GameStore.Core.Dto.Requests.Platform;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.Platforms;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class PlatformService(
        IUnitOfWork gameStoreUnitOfWork,
        IMapper mapper) : IPlatformService, IPlatformInternalService
    {
        public async Task<Result<PlatformResponse>> CreatePlatform(CreatePlatformRequest request, CancellationToken token = default)
        {
            var newPlatformType = request.Platform.Type;

            var spec = new PlatformByTypeSpecification(newPlatformType);
            var existingPlatform = await gameStoreUnitOfWork.Platforms.GetFirstOrDefaultAsync(spec, token: token);
            if (existingPlatform != null)
            {
                return Result.Failure<PlatformResponse>(PlatformErrors.AlreadyExists);
            }

            Platform platform = new(newPlatformType);

            await gameStoreUnitOfWork.Platforms.Add(platform);

            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return mapper.Map<PlatformResponse>(platform);
        }

        public async Task<Result> UpdatePlatform(UpdatePlatformRequest request, CancellationToken token = default)
        {
            var spec = new PlatformByIdSpecification(request.Platform.Id);
            var platformToUpdate = await gameStoreUnitOfWork.Platforms.GetFirstOrDefaultAsync(spec, token: token);

            if (platformToUpdate == null)
            {
                return Result.Failure(PlatformErrors.NotFound);
            }

            platformToUpdate.UpdateDetails(request.Platform.Type);

            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result> DeletePlatform(Guid platformId, CancellationToken token = default)
        {
            var spec = new PlatformByIdSpecification(platformId)
                .AsTracking();
            var platform = await gameStoreUnitOfWork.Platforms.GetFirstOrDefaultAsync(spec, token: token);

            if (platform == null)
            {
                return Result.Success();
            }

            await gameStoreUnitOfWork.Platforms.Delete(platform);

            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result<PlatformResponse>> GetPlatform(Guid platformId, CancellationToken token = default)
        {
            var spec = new PlatformByIdSpecification(platformId);
            var platform = await gameStoreUnitOfWork.Platforms.GetFirstOrDefaultAsync(spec, token: token);

            if (platform == null)
            {
                return Result.Failure<PlatformResponse>(PlatformErrors.NotFound);
            }

            return mapper.Map<PlatformResponse>(platform);
        }

        public async Task<IEnumerable<PlatformResponse>> GetAllPlatforms(CancellationToken token = default)
        {
            var spec = new AllPlatformsSpecification();
            var platforms = await gameStoreUnitOfWork.Platforms.GetListAsync(spec, token: token);

            return mapper.Map<IEnumerable<PlatformResponse>>(platforms);
        }

        public async Task<IEnumerable<PlatformResponse>> GetPlatformsByGameKey(string gameKey, CancellationToken token = default)
        {
            var spec = new PlatformByGameKeySpecification(gameKey);
            var platforms = await gameStoreUnitOfWork.Platforms.GetListAsync(spec, token);

            return mapper.Map<IEnumerable<PlatformResponse>>(platforms);
        }

        public async Task<IEnumerable<Platform>> GetPlatformsByIds(IEnumerable<Guid> requestPlatforms, CancellationToken token)
        {
            var ids = requestPlatforms.ToHashSet();

            if (ids.Count == 0)
                return [];

            var spec = new PlatformInRangeSpecification(ids).AsTracking();
            return await gameStoreUnitOfWork.Platforms.GetListAsync(spec, token);
        }
    }
}
