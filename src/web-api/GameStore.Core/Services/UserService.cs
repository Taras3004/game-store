using AutoMapper;
using GameStore.Core.Dto.Requests.User;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.Roles;
using GameStore.Core.Specifications.Users;
using GameStore.Models.Entities;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class UserService(
        IUnitOfWork gameStoreUnitOfWork,
        IMapper mapper,
        IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<Result<UserResponse>> CreateUser(CreateUserRequest request, CancellationToken token = default)
        {
            var spec = new UserByUsernameSpecification(request.User.Name);
            if (await gameStoreUnitOfWork.Users.AnyAsync(spec, token))
            {
                return Result.Failure<UserResponse>(UserErrors.UsernameAlreadyExists);
            }

            List<Role>? userRoles = null;

            if (request.Roles != null)
            {
                var resolveResult = await ResolveRolesByIds(request.Roles.ToHashSet(), token);
                if (resolveResult.IsFailure)
                    return Result.Failure<UserResponse>(resolveResult.Error);
                userRoles = resolveResult.Value;
            }

            var hashedPassword = passwordHasher.Hash(request.Password);
            var user = new User(request.User.Name, hashedPassword);

            if (userRoles != null)
            {
                user.SetRoles(userRoles);
            }

            await gameStoreUnitOfWork.Users.Add(user, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return mapper.Map<UserResponse>(user);
        }

        public async Task<Result<UserResponse>> UpdateUser(UpdateUserRequest request, CancellationToken token = default)
        {
            var userByNameSpec = new UserByUsernameSpecification(request.User.Name);
            var user = await gameStoreUnitOfWork.Users.GetFirstOrDefaultAsync(userByNameSpec, token);
            if (user != null && user.Id != request.User.Id)
            {
                return Result.Failure<UserResponse>(UserErrors.UsernameAlreadyExists);
            }

            var userByIdSpec = new UserByIdSpecification(request.User.Id).AsTracking();
            var userToUpdate = await gameStoreUnitOfWork.Users.GetFirstOrDefaultAsync(userByIdSpec, token);
            if (userToUpdate == null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound);
            }

            List<Role>? userRoles = null;

            if (request.Roles != null)
            {
                var resolveResult = await ResolveRolesByIds(request.Roles.ToHashSet(), token);
                if (resolveResult.IsFailure)
                    return Result.Failure<UserResponse>(resolveResult.Error);
                userRoles = resolveResult.Value;
            }

            var hashedPassword = passwordHasher.Hash(request.Password);
            userToUpdate.Update(request.User.Name, hashedPassword);
            if (userRoles != null)
            {
                userToUpdate.SetRoles(userRoles);
            }

            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return mapper.Map<UserResponse>(userToUpdate);
        }

        private async Task<Result<List<Role>>> ResolveRolesByIds(HashSet<Guid> rolesIds,
            CancellationToken token = default)
        {
            var spec = new RoleByIdInRangeSpecification(rolesIds)
                .AsTracking();
            var roles = (await gameStoreUnitOfWork.Roles.GetListAsync(spec, token)).ToList();
            if (roles.Count != rolesIds.Count)
            {
                return Result.Failure<List<Role>>(RoleErrors.NotFound);
            }

            return Result.Success(roles);
        }

        public async Task<Result> DeleteUser(Guid id, CancellationToken token = default)
        {
            var userByIdSpec = new UserByIdSpecification(id)
                .AsTracking();
            var user = await gameStoreUnitOfWork.Users.GetFirstOrDefaultAsync(userByIdSpec, token);
            if (user == null)
            {
                return Result.Success();
            }

            await gameStoreUnitOfWork.Users.Delete(user, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);
            return Result.Success();
        }

        public async Task<Result<UserResponse>> GetUserById(Guid id, CancellationToken token = default)
        {
            var userByIdSpec = new UserByIdSpecification(id);
            var user = await gameStoreUnitOfWork.Users.GetFirstOrDefaultAsync(userByIdSpec, token);
            if (user == null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound);
            }

            return mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers(CancellationToken token = default)
        {
            var users = await gameStoreUnitOfWork.Users.GetListAsync(new AllUsersSpecification(), token);
            return mapper.Map<IEnumerable<UserResponse>>(users);
        }
    }
}