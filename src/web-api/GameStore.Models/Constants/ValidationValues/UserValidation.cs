namespace GameStore.Models.Constants.ValidationValues
{
    public static class UserValidation
    {
        public const int UsernameMaxLength = 100;
        public const int PasswordMaxLength = 100;
        public const string RolesMustNotContainDuplicates = "Roles must not contain duplicates.";
        public const string PermissionPrefix = "Permission";
    }
}
