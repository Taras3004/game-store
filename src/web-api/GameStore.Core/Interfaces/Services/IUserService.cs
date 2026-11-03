using GameStore.Core.Dto.Requests.User;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserResponse>> CreateUser(CreateUserRequest request, CancellationToken token = default);
        Task<Result<UserResponse>> UpdateUser(UpdateUserRequest request, CancellationToken token = default);
        Task<Result> DeleteUser(Guid id, CancellationToken token = default);
        Task<Result<UserResponse>> GetUserById(Guid id, CancellationToken token = default);
        Task<IEnumerable<UserResponse>> GetAllUsers(CancellationToken token = default);
    }
}
