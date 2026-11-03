namespace GameStore.Core.Dto.Details.Order
{
    public record OrderFilterDetails
    {
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}
