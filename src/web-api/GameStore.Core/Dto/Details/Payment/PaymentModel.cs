namespace GameStore.Core.Dto.Details.Payment
{
    public record PaymentModel
    {
        public required string Holder { get; init; }
        public required string CardNumber { get; init; }
        public required int MonthExpire { get; init; }
        public required int YearExpire { get; init; }
        public required string Cvv2 { get; init; }
    }
}
