using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class GenreErrors
    {
        public static readonly Error NotFound = new(
            "Genre.NotFound",
            "The genre with the specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error AlreadyExists = new(
            "Genre.AlreadyExists",
            "The genre with the specified ID already exists",
            ErrorType.Conflict
        );

        public static readonly Error NameConflict = new(
            "Genre.NameConflict",
            "Genre with the same name already exists",
            ErrorType.Conflict
        );

        public static readonly Error ParentItselfConflict = new(
            "Genre.ParentItselfConflict",
            "Genre cannot be parent of itself",
            ErrorType.Conflict
        );

        public static readonly Error ParentNotFound = new(
            "Genre.ParentNotFound",
            "Parent genre with specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error CannotDeleteLegacyData = new(
            "Genre.CannotDeleteLegacyData",
            "Cannot delete legacy data",
            ErrorType.Conflict
        );
    }
}
