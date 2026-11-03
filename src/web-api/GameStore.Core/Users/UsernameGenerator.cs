namespace GameStore.Core.Users
{
    public static class UsernameGenerator
    {
        public static string GenerateUsername(string firstName, string lastName)
            => $"{firstName}_{lastName}";
    }
}
