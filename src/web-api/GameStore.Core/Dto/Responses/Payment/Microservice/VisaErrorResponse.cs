namespace GameStore.Core.Dto.Responses.Payment.Microservice
{
    public record VisaErrorResponse
    {
        public required string Title { get; init; }
        public int Status { get; init; }
    }
}
