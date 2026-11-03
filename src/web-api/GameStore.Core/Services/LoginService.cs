using GameStore.Core.Dto.Requests.User;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.Users;
using GameStore.Core.Users;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class LoginService(
        ITokenProvider tokenProvider, 
        IPasswordHasher passwordHasher,
        IUnitOfWork gameStoreUnitOfWork,
        ExternalAuthService externalAuthService) : ILoginService
    {
        public async Task<Result<LoginResponse>> Login(LoginUserRequest request, CancellationToken token = default)
        {
            var getUserResult = await ResolveUser(request, token);
            if (getUserResult.IsFailure)
                return Result.Failure<LoginResponse>(getUserResult.Error);
            var user = getUserResult.Value;

            if (!passwordHasher.Verify(request.Model.Password, user.Password))
                return Result.Failure<LoginResponse>(UserErrors.PasswordIsNotCorrect);

            var jwt = tokenProvider.CreateToken(user);

            return new LoginResponse()
            {
                Token = jwt
            };
        }

        private async Task<Result<User>> ResolveUser(LoginUserRequest request, CancellationToken token = default)
        {
            User? user;

            if (request.Model.InternalAuth)
            {
                var login = request.Model.Login;
                var spec = new UserByUsernameSpecification(login)
                    .WithRoles();
                user = await gameStoreUnitOfWork.Users.GetFirstOrDefaultAsync(spec, token);
                if(user is null)
                    return Result.Failure<User>(UserErrors.NotFound);
            }
            else
            {
                var getUserResult = await externalAuthService.AuthenticateUser(request.Model.Login, request.Model.Password);
                if (getUserResult.IsFailure)
                    return Result.Failure<User>(getUserResult.Error);
                
                var externalUserData = getUserResult.Value;
                var username =
                    UsernameGenerator.GenerateUsername(externalUserData.FirstName, externalUserData.LastName);

                var hashedPassword = passwordHasher.Hash(request.Model.Password);
                user = new User(username, hashedPassword, externalUserData.Email);
                await gameStoreUnitOfWork.Users.Add(user, token);
                await gameStoreUnitOfWork.SaveChangesAsync(token);
            }

            return user;
        }
    }
}
