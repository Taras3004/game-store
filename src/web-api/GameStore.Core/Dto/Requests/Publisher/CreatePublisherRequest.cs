using GameStore.Core.Dto.Details.Publisher;

namespace GameStore.Core.Dto.Requests.Publisher
{
    public record CreatePublisherRequest
    {
        public required PublisherDetailsBase Publisher { get; init; }
    }
}
