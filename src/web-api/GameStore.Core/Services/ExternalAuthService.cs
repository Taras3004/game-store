using GameStore.Core.Dto.Responses.AuthService;
using GameStore.Core.Users;
using GameStore.Models.Errors;
using GameStore.Models.Results;
using System.Net.Http.Json;

namespace GameStore.Core.Services
{
    internal class ExternalAuthService(HttpClient httpClient)
    {
        public async Task<Result<AuthServiceUserResponse>> AuthenticateUser(string login, string password)
        {
            var usersResponse = await httpClient.GetAsync("users");

            if (!usersResponse.IsSuccessStatusCode)
                return Result.Failure<AuthServiceUserResponse>(UserErrors.UnknownFailure);

            var users = usersResponse.Content.ReadFromJsonAsAsyncEnumerable<AuthServiceUserResponse>();

            AuthServiceUserResponse? foundedUser = null;
            await foreach (var user in users)
            {
                if (user is null)
                    continue;

                if (user.Email == login ||
                    UsernameGenerator.GenerateUsername(user.FirstName, user.LastName) == login)
                {
                    foundedUser = user;
                    break;
                }
            }

            if (foundedUser is null)
                return Result.Failure<AuthServiceUserResponse>(UserErrors.NotFound);

            var authPayload = new
            {
                email = foundedUser.Email,
                password = password
            };

            var authResponse = await httpClient.PostAsJsonAsync("auth", authPayload);

            if (!authResponse.IsSuccessStatusCode)
                return Result.Failure<AuthServiceUserResponse>(UserErrors.NotFound);

            return Result.Success(foundedUser);
        }
    }
}