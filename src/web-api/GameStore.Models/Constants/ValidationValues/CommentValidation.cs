namespace GameStore.Models.Constants.ValidationValues
{
    public static class CommentValidation
    {
        public const int NameMaxLength = 100;
        public const int BodyMaxLength = 500;
        public const string DeletedCommentText = "A comment/quote was deleted";
        public const string InvalidType = "Invalid comment type";
        public const string ParentIdMustBeNull = "ParentId must be null for a standard comment";
        public const string ParentIdIsRequired = "ParentId is required for actions";
    }
}
