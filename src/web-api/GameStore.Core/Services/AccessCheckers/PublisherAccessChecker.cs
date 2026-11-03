using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class PublisherAccessChecker(IPublisherService publisherService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await publisherService.GetPublisher(targetId, token);

            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
