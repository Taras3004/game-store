namespace GameStore.Core.Specifications.Roles
{
    internal class RoleByNameSpecification(string name) : RoleSpecificationBase(role => role.Name == name);
}
