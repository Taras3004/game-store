namespace GameStore.Core.Dto.Details.Comment
{
    public record CommentDetailsBase
    {
        public required string Name { get; init; }
        public required string Body { get; init; }
    }
}