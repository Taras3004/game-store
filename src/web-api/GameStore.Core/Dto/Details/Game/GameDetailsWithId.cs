namespace GameStore.Core.Dto.Details.Game
{
    public record GameDetailsWithId : GameDetailsBase
    {
        public required Guid Id { get; init; }
    }
}