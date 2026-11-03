using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class PlatformErrors
    {
        public static readonly Error NotFound = new(
            "Platform.NotFound",
            "The platform with the specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error AlreadyExists = new(
            "Platform.AlreadyExists",
            "The platform with the specified ID already exists",
            ErrorType.Conflict
        );
    }
}
