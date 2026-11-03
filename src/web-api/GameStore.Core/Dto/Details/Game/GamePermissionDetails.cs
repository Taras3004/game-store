namespace GameStore.Core.Dto.Details.Game
{
    public record GamePermissionDetails
    {
        public bool AccessDeleted { get; init; }
        public bool AccessOutOfStock { get; init; }
    }
}