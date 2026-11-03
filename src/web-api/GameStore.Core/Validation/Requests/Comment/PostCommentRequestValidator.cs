using FluentValidation;
using GameStore.Core.Dto.Requests.Comment;
using GameStore.Core.Validation.Details.Comment;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Enums;

namespace GameStore.Core.Validation.Requests.Comment
{
    public class PostCommentRequestValidator : AbstractValidator<PostCommentRequest>
    {
        public PostCommentRequestValidator()
        {
            RuleFor(x => x.Comment)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new CommentDetailsBaseValidator());

            RuleFor(x => x.Action)
                        .IsEnumName(typeof(CommentType))
                        .Must(x => x != nameof(CommentType.Default))
                        .WithMessage(CommentValidation.InvalidType)
                        .When(x => !string.IsNullOrWhiteSpace(x.Action));

            RuleFor(x => x.ParentId)
                .Null()
                .When(x => x.Action is null)
                .WithMessage(CommentValidation.ParentIdMustBeNull);

            RuleFor(x => x.ParentId)
                .NotNull()
                .When(x => x.Action is not null)
                .WithMessage(CommentValidation.ParentIdIsRequired);
        }
    }
}

