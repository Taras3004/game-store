namespace GameStore.Core.Specifications.Roles
{
    internal class RoleByIdInRangeSpecification(IReadOnlyCollection<Guid> ids)
        : RoleSpecificationBase(role => ids.Contains(role.Id));
}
