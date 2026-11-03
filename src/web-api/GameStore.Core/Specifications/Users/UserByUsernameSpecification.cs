namespace GameStore.Core.Specifications.Users
{
    internal class UserByUsernameSpecification(string username)
        : UserSpecificationBase(user => user.Username == username);
}
