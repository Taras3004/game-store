using GameStore.Core.Dto.Requests.User;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface ILoginService
    {
        Task<Result<LoginResponse>> Login(LoginUserRequest request, CancellationToken token = default);
    }
}
