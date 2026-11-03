using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class DomainErrors
    {
        public static readonly Error SaveError = new(
            "Domain.SaveError",
            "Error o",
            ErrorType.NotFound
        );
    }
}
