using AutoMapper;
using GameStore.Core.Dto.Requests.Role;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.Roles;
using GameStore.Core.Specifications.Users;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class RoleService(
        IUnitOfWork gameStoreUnitOfWork,
        IMapper mapper) : IRoleService
    {
        public async Task<Result<RoleResponse>> CreateRole(CreateRoleRequest request, CancellationToken token = default)
        {
            var spec = new RoleByNameSpecification(request.Role.Name);
            if (await gameStoreUnitOfWork.Roles.AnyAsync(spec, token))
            {
                return Result.Failure<RoleResponse>(RoleErrors.NameAlreadyExists);
            }

            var role = new Role(request.Role.Name);

            role.SetPermissions(request.Permissions.ToList());

            await gameStoreUnitOfWork.Roles.Add(role);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return mapper.Map<RoleResponse>(role);
        }

        public async Task<Result<RoleResponse>> UpdateRole(UpdateRoleRequest request, CancellationToken token = default)
        {
            var roleByIdSpec = new RoleByIdSpecification(request.Role.Id).AsTracking();

            var roleToUpdate = await gameStoreUnitOfWork.Roles.GetFirstOrDefaultAsync(roleByIdSpec, token);
            if (roleToUpdate == null)
            {
                return Result.Failure<RoleResponse>(RoleErrors.NotFound);
            }

            var roleByNameSpec = new RoleByNameSpecification(request.Role.Name);
            var role = await gameStoreUnitOfWork.Roles.GetFirstOrDefaultAsync(roleByNameSpec, token);

            if (role != null && role.Id != roleToUpdate.Id)
            {
                return Result.Failure<RoleResponse>(RoleErrors.NameAlreadyExists);
            }

            roleToUpdate.Update(request.Role.Name);

            roleToUpdate.SetPermissions(request.Permissions.ToList());

            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return mapper.Map<RoleResponse>(roleToUpdate);
        }

        public async Task<Result> DeleteRole(Guid id, CancellationToken token = default)
        {
            var roleByIdSpec = new RoleByIdSpecification(id).AsTracking();
            var role = await gameStoreUnitOfWork.Roles.GetFirstOrDefaultAsync(roleByIdSpec, token);
            if (role == null)
                return Result.Success();

            await gameStoreUnitOfWork.Roles.Delete(role);
            await gameStoreUnitOfWork.SaveChangesAsync(token);
            return Result.Success();
        }

        public async Task<Result<RoleResponse>> GetRoleById(Guid id, CancellationToken token = default)
        {
            var spec = new RoleByIdSpecification(id);
            var role = await gameStoreUnitOfWork.Roles.GetFirstOrDefaultAsync(spec, token);

            if (role is null)
                return Result.Failure<RoleResponse>(RoleErrors.NotFound);

            return mapper.Map<RoleResponse>(role);
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRoles(CancellationToken token = default)
        {
            var roles = await gameStoreUnitOfWork.Roles.GetListAsync(new AllRolesSpecification(), token);
            return mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<Result<IEnumerable<Permission>>> GetRolePermissionsById(Guid id,
            CancellationToken token = default)
        {
            var roleByIdSpec = new RoleByIdSpecification(id).AsTracking();
            var role = await gameStoreUnitOfWork.Roles.GetFirstOrDefaultAsync(roleByIdSpec, token);
            if (role == null)
                return Result.Failure<IEnumerable<Permission>>(RoleErrors.NotFound);

            return Result.Success((IEnumerable<Permission>)role.Permissions);
        }

        public IEnumerable<string> GetAllPermissions()
        {
            return Enum.GetNames(typeof(Permission));
        }

        public async Task<Result<IEnumerable<string>>> GetPermissionsByRoleId(Guid roleId, CancellationToken token = default)
        {
            var spec = new RoleByIdSpecification(roleId);
            var role = await gameStoreUnitOfWork.Roles.GetFirstOrDefaultAsync(spec, token);

            if (role is null)
                return Result.Failure<IEnumerable<string>>(RoleErrors.NotFound);

            var permissions = role.Permissions.Select(x => x.ToString());

            return Result.Success(permissions);
        }

        public async Task<Result<IEnumerable<RoleResponse>>> GetRolesByUserId(Guid userId, CancellationToken token = default)
        {
            var spec = new UserByIdSpecification(userId)
                .WithRoles();
            var user = await gameStoreUnitOfWork.Users.GetFirstOrDefaultAsync(spec, token);
            if (user == null)
                return Result.Failure<IEnumerable<RoleResponse>>(UserErrors.NotFound);

            return Result.Success(mapper.Map<IEnumerable<RoleResponse>>(user.Roles));
        }
    }
}