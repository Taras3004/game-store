namespace GameStore.Core.Specifications.Users
{
    internal class UserByIdSpecification(Guid id) 
        : UserSpecificationBase(user => user.Id == id);
}
