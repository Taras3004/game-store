namespace GameStore.Core.Dto.Responses
{
    public record CommentResponse
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Body { get; init; }
        public required IEnumerable<CommentResponse> ChildComments { get; init; }
    }
}
