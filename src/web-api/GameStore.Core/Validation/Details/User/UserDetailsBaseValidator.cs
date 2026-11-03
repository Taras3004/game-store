using FluentValidation;
using GameStore.Core.Dto.Details.User;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Details.User
{
    public class UserDetailsBaseValidator : AbstractValidator<UserDetailsBase>
    {
        public UserDetailsBaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            int nameMaxLength = UserValidation.UsernameMaxLength;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(nameMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);
        }
    }
}
