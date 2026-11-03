using GameStore.Core.Dto.Requests.Ban;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.BanRecords;
using GameStore.Core.Users;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Exceptions;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class BanService(
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork gameStoreUnitOfWork
    ) : IBanService
    {
        public async Task<Result> BanUser(BanUserRequest request, CancellationToken token = default)
        {
            if (UserMappings.BanDurationMap.TryGetValue(request.Duration, out TimeSpan banDuration))
            {
                DateTime bannedUntil = GetUnbanDateTime(banDuration, dateTimeProvider.UtcNow);

                var spec = new ActiveBanRecordByUsernameSpecification(request.User, dateTimeProvider.UtcNow);
                var banRecord = await gameStoreUnitOfWork.BanRecords.GetFirstOrDefaultAsync(spec, token: token);

                if (banRecord is null)
                {
                    BanRecord newBanRecord = new(request.User, request.Duration, bannedUntil);

                    await gameStoreUnitOfWork.BanRecords.Add(newBanRecord);
                }
                else
                {
                    banRecord.UpdateRecord(bannedUntil);
                }

                try
                {
                    await gameStoreUnitOfWork.SaveChangesAsync(token);
                }
                catch (UpdateConcurrencyException)
                {
                    return Result.Failure(BanErrors.ConcurrentUpdate);
                }
                catch (UpdateException)
                {
                    return Result.Failure(BanErrors.UserIsBanned);
                }

                return Result.Success();
            }

            return Result.Failure(BanErrors.OptionNotFound);
        }

        private static DateTime GetUnbanDateTime(TimeSpan banDuration, DateTime currentTime)
        {
            if (banDuration == TimeSpan.MaxValue)
            {
                return DateTime.MaxValue;
            }

            return currentTime + banDuration;
        }

        public async Task<bool> IsUserBanned(string username, CancellationToken token = default)
        {
            var spec = new ActiveBanRecordByUsernameSpecification(username, dateTimeProvider.UtcNow);
            return await gameStoreUnitOfWork.BanRecords.AnyAsync(spec, token: token);
        }

        public IEnumerable<string> GetBanDurations()
        {
            return UserMappings.BanDurationMap.Keys;
        }
    }
}