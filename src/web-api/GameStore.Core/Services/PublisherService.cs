using AutoMapper;
using GameStore.Core.Dto.Requests.Publisher;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class PublisherService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IPublisherService
    {
        public async Task<Result<PublisherResponse>> CreatePublisher(CreatePublisherRequest request, CancellationToken token = default)
        {
            if (await unitOfWork.Publishers.IsNameTakenAsync(request.Publisher.CompanyName, token))
            {
                return Result.Failure<PublisherResponse>(PublisherErrors.CompanyNameAlreadyTaken);
            }

            var publisher = mapper.Map<Publisher>(request.Publisher);

            await unitOfWork.Publishers.AddAsync(publisher, token);
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success(mapper.Map<PublisherResponse>(publisher));
        }

        public async Task<Result> UpdatePublisher(UpdatePublisherRequest request, CancellationToken token = default)
        {
            if (await unitOfWork.Publishers.IsNameTakenExcludeIdAsync(request.Publisher.CompanyName, request.Publisher.Id, token))
            {
                return Result.Failure(PublisherErrors.CompanyNameAlreadyTaken);
            }

            var publisher = await unitOfWork.Publishers.GetByIdAsync(request.Publisher.Id, TrackingBehaviour.Tracking, token);

            if (publisher == null)
            {
                return Result.Failure(PublisherErrors.NotFound);
            }

            mapper.Map(request.Publisher, publisher);

            await unitOfWork.Publishers.UpdateAsync(publisher, token);
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result> DeletePublisher(Guid id, CancellationToken token = default)
        {
            var publisher = await unitOfWork.Publishers.GetByIdAsync(id, TrackingBehaviour.Tracking, token);
            if(publisher is null)
            {
                return Result.Failure(PublisherErrors.NotFound);
            }

            if (!string.IsNullOrEmpty(publisher.LegacyId))
            {
                return Result.Failure(PublisherErrors.CannotDeleteLegacyData);
            }

            await unitOfWork.Publishers.DeleteAsync(publisher, token);
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result<PublisherResponse>> GetPublisher(Guid id, CancellationToken token = default)
        {
            var publisher = await unitOfWork.Publishers.GetByIdAsync(id, token: token);

            if (publisher is null)
            {
                return Result.Failure<PublisherResponse>(PublisherErrors.NotFound);
            }

            return mapper.Map<PublisherResponse>(publisher);
        }

        public async Task<Result<PublisherResponse>> GetPublisherByCompanyName(string companyName, CancellationToken token = default)
        {
            var publisher = await unitOfWork.Publishers.GetByCompanyNameAsync(companyName, token: token);

            if (publisher is null)
            {
                return Result.Failure<PublisherResponse>(PublisherErrors.NotFound);
            }

            return mapper.Map<PublisherResponse>(publisher);
        }

        public async Task<IEnumerable<PublisherResponse>> GetAllPublishers(CancellationToken token = default)
        {
            var publishers = await unitOfWork.Publishers.GetAllAsync(token);
            return mapper.Map<IEnumerable<PublisherResponse>>(publishers);
        }

        public async Task<Result<PublisherResponse>> GetPublisherByGameKey(string gameKey, CancellationToken token = default)
        {
            var publisher = await unitOfWork.Publishers.GetByGameKeyAsync(gameKey, token: token);

            if (publisher is null)
            {
                return Result.Failure<PublisherResponse>(PublisherErrors.NotFound);
            }

            return mapper.Map<PublisherResponse>(publisher);
        }
    }
}