using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class CommentErrors
    {
        public static readonly Error NotFound = new(
            "Comment.NotFound",
            "The comment with the specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error ParentNotFound = new(
            "Comment.ParentNotFound",
            "The parent comment with the specified ID was not found",
            ErrorType.NotFound
        );

        public static readonly Error InvalidActionForChildComment = new(
            "Comment.InvalidActionForChildComment",
            "Invalid Action For Child Comment",
            ErrorType.Conflict
        );

        public static readonly Error ParentIdRequired = new(
            "Comment.ParentIdRequired",
            "The parent orderId is required for this action",
            ErrorType.Conflict
        );

        public static readonly Error ConcurrentCreation = new(
            "Comment.ConcurrentCreation",
            "Another comment was created concurrently",
            ErrorType.Conflict
        );

        public static readonly Error Forbidden = new(
            "Comment.Forbidden",
            "Current user cannot perform action on this comment",
            ErrorType.Conflict
            );
    }
}