using GameStore.Core.Dto.Details.Publisher;

namespace GameStore.Core.Dto.Requests.Publisher
{
    public record UpdatePublisherRequest
    {
        public required PublisherDetailsWithId Publisher { get; init; }
    }
}
