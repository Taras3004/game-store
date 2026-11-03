using GameStore.Core.Dto.Requests.Publisher;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IPublisherService
    {
        Task<Result<PublisherResponse>> CreatePublisher(CreatePublisherRequest request, CancellationToken token = default);
        Task<Result> UpdatePublisher(UpdatePublisherRequest request, CancellationToken token = default);
        Task<Result> DeletePublisher(Guid id, CancellationToken token = default);
        Task<Result<PublisherResponse>> GetPublisher(Guid id, CancellationToken token = default);
        Task<Result<PublisherResponse>> GetPublisherByCompanyName(string companyName, CancellationToken token = default);
        Task<IEnumerable<PublisherResponse>> GetAllPublishers(CancellationToken token = default);
        Task<Result<PublisherResponse>> GetPublisherByGameKey(string gameKey, CancellationToken token = default);
    }
}
