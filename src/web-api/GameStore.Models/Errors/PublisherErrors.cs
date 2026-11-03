using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class PublisherErrors
    {
        public static readonly Error NotFound = new(
            "Publisher.NotFound",
            "The publisher with the specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error AlreadyExists = new(
            "Publisher.AlreadyExists",
            "The publisher with the specified ID already exists",
            ErrorType.Conflict
        );

        public static readonly Error CompanyNameAlreadyTaken = new(
            "Publisher.CompanyNameTaken",
            "The specified company name is already taken",
            ErrorType.Conflict
        );

        public static readonly Error CannotDeleteLegacyData = new(
            "Publisher.CannotDeleteLegacyData",
            "Cannot delete legacy data",
            ErrorType.Conflict
        );
    }
}
