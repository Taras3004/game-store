namespace GameStore.Core.Specifications.Roles
{
    internal class RoleByIdSpecification(Guid id) : RoleSpecificationBase(role => role.Id == id);
}
