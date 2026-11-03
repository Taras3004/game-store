namespace GameStore.Core.Dto.Details.Platform
{
    public record PlatformDetailsWithId : PlatformDetailsBase
    {
        public required Guid Id { get; init; }
    }
}