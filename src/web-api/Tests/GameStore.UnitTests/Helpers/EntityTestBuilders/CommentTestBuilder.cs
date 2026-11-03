using GameStore.Core.Dto.Details.Comment;
using GameStore.Core.Dto.Requests.Comment;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class CommentTestBuilder
    {
        public static Comment CreateValidRootComment(
            string name = EntityBuilderConstants.ProvidedByBuilderString, 
            string body = EntityBuilderConstants.ProvidedByBuilderString, 
            string productKey = EntityBuilderConstants.ProvidedByBuilderString,
            Guid? customerId = null)
        {
            customerId ??= Guid.Empty;
            return new Comment(
                name, body, productKey, customerId.Value);
        }

        public static Comment CreateValidChildComment(
            string name = EntityBuilderConstants.ProvidedByBuilderString, 
            string body = EntityBuilderConstants.ProvidedByBuilderString, 
            CommentType commentType = CommentType.Reply, 
            Comment? parentComment = null)
        {
            return new Comment(
                name, body, commentType, parentComment!);
        }

        public static PostCommentRequest CreateValidPostRootCommentRequest(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string body = EntityBuilderConstants.ProvidedByBuilderString)
        {
            return new PostCommentRequest()
            {
                Comment = CreateValidCommentDetailsBase(name, body),
                ParentId = null,
                Action = null
            };
        }

        public static PostCommentRequest CreateValidPostChildCommentRequest(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string body = EntityBuilderConstants.ProvidedByBuilderString,
            Guid parentId = default,
            string? action = nameof(CommentType.Reply))
        {
            return new PostCommentRequest()
            {
                Comment = CreateValidCommentDetailsBase(name, body),
                ParentId = parentId,
                Action = action
            };
        }

        public static CommentDetailsBase CreateValidCommentDetailsBase(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string body = EntityBuilderConstants.ProvidedByBuilderString)
        {
            return new CommentDetailsBase
            {
                Name = name,
                Body = body
            };
        }
    }
}
