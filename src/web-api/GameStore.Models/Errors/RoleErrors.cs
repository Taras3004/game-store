using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class RoleErrors
    {
        public static readonly Error NotFound = new(
            "Role.NotFound",
            "The role was not found",
            ErrorType.NotFound
        );

        public static readonly Error NameAlreadyExists = new(
            "Role.NameAlreadyExists",
            "The role with the specified name is already exists",
            ErrorType.Conflict
        );
    }
}
