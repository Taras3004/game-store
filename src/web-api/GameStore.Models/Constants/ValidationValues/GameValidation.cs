namespace GameStore.Models.Constants.ValidationValues
{
    public static class GameValidation
    {
        public const int NameMaxLength = 100;
        public const int KeyMaxLength = 100;
        public const int DescriptionMaxLength = 100;
        public const string GenresMustNotContainDuplicates = "Genres must not contain duplicates.";
        public const string PlatformsMustNotContainDuplicates = "Platforms must not contain duplicates.";
        public const int MinNameFilterChars = 3;
    }
}
