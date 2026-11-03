using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class BanErrors
    {
        public static readonly Error OptionNotFound = new(
            "Ban.OptionNotFound",
            "Ban option with this name was not found",
            ErrorType.NotFound
        );

        public static readonly Error UserIsBanned = new(
            "Ban.UserIsBanned",
            "User has active ban record",
            ErrorType.Conflict
        );

        public static readonly Error ConcurrentUpdate = new(
            "Ban.ConcurrentUpdate",
            "Ban record was updated concurrently",
            ErrorType.Conflict
        );
    }
}