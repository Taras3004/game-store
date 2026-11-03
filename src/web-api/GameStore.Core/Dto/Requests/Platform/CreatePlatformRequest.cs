using GameStore.Core.Dto.Details.Platform;

namespace GameStore.Core.Dto.Requests.Platform
{
    public record CreatePlatformRequest
    {
        public required PlatformDetailsBase Platform { get; init; }
    }
}
