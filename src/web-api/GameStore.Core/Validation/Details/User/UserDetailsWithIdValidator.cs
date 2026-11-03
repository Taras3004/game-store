using FluentValidation;
using GameStore.Core.Dto.Details.User;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.User
{
    public class UserDetailsWithIdValidator : AbstractValidator<UserDetailsWithId>
    {
        public UserDetailsWithIdValidator()
        {
            Include(new UserDetailsBaseValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}
