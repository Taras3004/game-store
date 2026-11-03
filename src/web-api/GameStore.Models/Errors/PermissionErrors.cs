using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class PermissionErrors
    {
        public static readonly Error NotFound = new(
            "Permission.NotFound",
            "The permission was not found",
            ErrorType.NotFound
        );
    }
}
