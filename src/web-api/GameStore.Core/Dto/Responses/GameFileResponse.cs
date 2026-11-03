namespace GameStore.Core.Dto.Responses
{
    public record GameFileResponse
    {
        public required byte[] FileContent { get; init; }
        public required string ContentType { get; init; }
        public required string FileName { get; init; }
    }
}