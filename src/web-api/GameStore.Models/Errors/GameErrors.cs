using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class GameErrors
    {
        public static readonly Error NotFound = new(
            "Game.NotFound",
            "The game with the specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error AlreadyExists = new(
            "Game.AlreadyExists",
            "The game with the specified ID already exists",
            ErrorType.Conflict
        );

        public static readonly Error KeyAlreadyTaken = new(
            "Game.KeyAlreadyTaken",
            "The specified key is already taken",
            ErrorType.Conflict
        );

        public static readonly Error RelatedGenreNotFound = new(
            "Game.RelatedGenreNotFound",
            "The genre with the specified orderId is not related to the game",
            ErrorType.NotFound
        );

        public static readonly Error RelatedPlatformNotFound = new(
            "Game.RelatedPlatformNotFound",
            "The platform with the specified orderId is not related to the game",
            ErrorType.NotFound
        );

        public static readonly Error PublisherNotFound = new(
            "Game.PublisherNotFound",
            "The game with the specified key do not have a publisher",
            ErrorType.NotFound
        );

        public static readonly Error PriceBelowZero = new(
            "Game.PriceBelowZero",
            "The price cannot be negative",
            ErrorType.InvalidData
        );

        public static readonly Error DiscountOutOfRange = new(
            "Game.DiscountOutOfRange",
            "The discount should be from 0 to 100 percent",
            ErrorType.InvalidData
        );

        public static readonly Error OutOfStock = new(
            "Game.OutOfStock",
            "The game is out of stock",
            ErrorType.Conflict
        );

        public static readonly Error ConcurrentUpdate = new(
            "Game.ConcurrencyUpdate",
            "The game have not been updated due to race condition",
            ErrorType.Conflict
        );

        public static readonly Error NameFilterTooSmall = new(
            "Game.NameFilterTooSmall",
            "Filter should have minimum 3 symbols",
            ErrorType.InvalidData
        );

        public static readonly Error CannotDeleteLegacyData = new(
            "Game.CannotDeleteLegacyData",
            "Cannot delete legacy data",
            ErrorType.Conflict
        );

        public static readonly Error ModificationForbidden = new(
            "Game.ModificationForbidden",
            "Current user do not have permission to modify this game",
            ErrorType.Conflict
        );
    }
}
