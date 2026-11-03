using FluentValidation;
using GameStore.Core.Dto.Details.Comment;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Comment
{
    public class CommentDetailsBaseValidator : AbstractValidator<CommentDetailsBase>
    {
        public CommentDetailsBaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);

            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}