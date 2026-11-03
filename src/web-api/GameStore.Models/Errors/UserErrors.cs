using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class UserErrors
    {
        public static readonly Error NotFound = new(
            "User.NotFound",
            "The user was not found",
            ErrorType.NotFound
        );

        public static readonly Error UsernameAlreadyExists = new(
            "User.UsernameAlreadyExists",
            "The user with the specified username already exists",
            ErrorType.NotFound
        );

        public static readonly Error PasswordIsNotCorrect = new(
            "User.PasswordIsNotCorrect",
            "Provided password is not correct",
            ErrorType.NotFound
        );

        public static readonly Error UnknownFailure = new(
            "User.UnknownFailure",
            "Unknown failure. Please try again",
            ErrorType.NotFound
        );

        public static readonly Error NotAuthenticated = new(
            "User.NotAuthenticated",
            "Not Authenticated. Login and try again",
            ErrorType.NotFound
        );
    }
}
