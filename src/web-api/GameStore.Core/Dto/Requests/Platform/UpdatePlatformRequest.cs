using GameStore.Core.Dto.Details.Platform;

namespace GameStore.Core.Dto.Requests.Platform
{
    public record UpdatePlatformRequest()
    {
        public required PlatformDetailsWithId Platform { get; init; }
    }
}
