namespace GameStore.Core.Validation.Helpers
{
    internal static class ValidationMessages
    {
        public const string CanNotBeEmpty 
            = "'{PropertyName}' can not be empty";

        public const string CanNotBeNull
            = "'{PropertyName}' can not be null";

        public const string LengthHigherThanMaxValue
            = "'{PropertyName}' length is higher than max value ({MaxLength} symbols)";
    }
}
