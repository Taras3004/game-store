using AutoMapper;
using GameStore.Core.Dto.Details.Comment;
using GameStore.Core.Dto.Requests.Comment;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.Comments;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.DomainEvents.Events.Comments;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class CommentService(
        IGameService gameService,
        IBanService banService,
        IUnitOfWork gameStoreUnitOfWork,
        IMapper mapper,
        IUserProvider userProvider) : ICommentService
    {
        public async Task<Result> PostComment(PostCommentRequest request, string gameKey, CancellationToken token = default)
        {
            if (await banService.IsUserBanned(request.Comment.Name, token))
            {
                return Result.Failure(BanErrors.UserIsBanned);
            }
            
            if (!await gameStoreUnitOfWork.Games.IsKeyTakenAsync(gameKey, token: token))
            {
                return Result.Failure(GameErrors.NotFound);
            }

            var createCommentResult = await CreateCommentAndSave(request, gameKey, token);

            if (createCommentResult.IsFailure)
            {
                return Result.Failure(createCommentResult.Error);
            }

            return Result.Success();
        }

        public async Task<Result<IEnumerable<CommentResponse>>> GetComments(string gameKey, CancellationToken token = default)
        {
            var gameResult = await gameService.GetGameByKey(gameKey, token);
            if (gameResult.IsFailure)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(GameErrors.NotFound);
            }

            var permissions = GetViewPermissionDetails();
            var spec = new CommentByGameKeySpecification(gameKey, permissions)
                .AsNoTrackingWithIdentityResolution();

            var comments = await gameStoreUnitOfWork.Comments.GetListAsync(spec, token: token);

            var rootComments = comments.Where(x => x.CommentType == CommentType.Default);
            var mappedComments = MapComments(rootComments);

            return Result.Success<IEnumerable<CommentResponse>>(mappedComments);
        }

        public async Task<Result> DeleteComment(string gameKey, Guid commentId, CancellationToken token = default)
        {
            var gameResult = await gameService.GetGameByKey(gameKey, token);
            if (gameResult.IsFailure)
            {
                return Result.Failure(GameErrors.NotFound);
            }

            var commentSpec = new CommentByIdSpecification(commentId).AsTracking();
            var comment = await gameStoreUnitOfWork.Comments.GetFirstOrDefaultAsync(commentSpec, token: token);

            if (comment is null)
            {
                return Result.Success();
            }

            if (!CanDeleteComment(comment))
            {
                return Result.Failure(CommentErrors.Forbidden);
            }

            comment.Delete();

            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result<CommentResponse>> GetCommentById(Guid id, CancellationToken token = default)
        {
            var permissions = GetViewPermissionDetails();
            var spec = new CommentByIdSpecification(id, permissions);
            var comment = await gameStoreUnitOfWork.Comments.GetFirstOrDefaultAsync(spec, token);

            if (comment is null)
            {
                return Result.Failure<CommentResponse>(CommentErrors.NotFound);
            }

            var gameResult = await gameService.GetGameByKey(comment.ProductKey, token);

            if (gameResult.IsFailure)
            {
                return Result.Failure<CommentResponse>(CommentErrors.NotFound);
            }

            return mapper.Map<CommentResponse>(comment);
        }

        private bool CanDeleteComment(Comment comment)
        {
            if (userProvider.HasPermission(Permission.DeleteAnyComment))
            {
                return true;
            }

            return userProvider.HasPermission(Permission.DeleteOwnComment)
                   && comment.UserId == userProvider.GetUserId();
        }

        private async Task<Result> CreateCommentAndSave(PostCommentRequest request, string productKey, CancellationToken token)
        {
            var userId = userProvider.GetUserId();
            if (!userProvider.IsAuthenticated || userId is null)
                return Result.Failure(UserErrors.NotAuthenticated);

            Comment comment;

            if (request.ParentId.HasValue)
            {
                var parentCommentSpec = new CommentByIdSpecification(request.ParentId.Value)
                    .AsTracking();
                var parentComment = await gameStoreUnitOfWork.Comments.GetFirstOrDefaultAsync(parentCommentSpec, token: token);

                if (parentComment is null)
                {
                    return Result.Failure<Comment>(CommentErrors.ParentNotFound);
                }

                CommentType? commentType = null;

                if (Enum.TryParse(request.Action, out CommentType parsedAction)
                    && parsedAction != CommentType.Default)
                {
                    commentType = parsedAction;
                }

                if (commentType is null)
                {
                    return Result.Failure<Comment>(CommentErrors.InvalidActionForChildComment);
                }

                comment = new Comment(request.Comment.Name, request.Comment.Body, commentType.Value, parentComment);
            }
            else
            {
                comment = new Comment(request.Comment.Name, request.Comment.Body, productKey, userId.Value);
            }

            comment.Raise(new CommentCreatedEvent(productKey));

            await gameStoreUnitOfWork.Comments.Add(comment, token);
            await gameStoreUnitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        private static List<CommentResponse> MapComments(IEnumerable<Comment> comments)
        {
            List<CommentResponse> commentResponses = [];
            foreach (Comment comment in comments)
            {
                commentResponses.Add(new CommentResponse
                {
                    Id = comment.Id,
                    Name = comment.Name,
                    Body = MaskBodyIfDeleted(comment),
                    ChildComments = MapComments(comment.ChildComments)
                });
            }

            return commentResponses;
        }

        private static string MaskBodyIfDeleted(Comment comment)
        {
            if (comment.IsDeleted)
            {
                return CommentValidation.DeletedCommentText;
            }

            return comment.CommentType switch
            {
                CommentType.Default => comment.Body,
                CommentType.Reply => $"[{comment.ParentComment!.Name}], {comment.Body}",
                CommentType.Quote => $"[{MaskBodyIfDeleted(comment.ParentComment!)}], {comment.Body}",
                _ => throw new InvalidOperationException(),
            };
        }

        private CommentPermissionDetails GetViewPermissionDetails()
        {
            return new CommentPermissionDetails()
            {
                AccessDeleted = userProvider.HasPermission(Permission.ViewDeletedComment)
            };
        }
    }
}