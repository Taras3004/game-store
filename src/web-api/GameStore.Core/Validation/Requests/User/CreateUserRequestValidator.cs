using FluentValidation;
using GameStore.Core.Dto.Requests.User;
using GameStore.Core.Validation.Details.User;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Requests.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.User)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new UserDetailsBaseValidator());


            RuleForEach(x => x.Roles)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);

            RuleFor(x => x.Roles)
                .Must(list =>
                {
                    IEnumerable<Guid> ids = list!.ToList();
                    return ids.Distinct().Count() == ids.Count();
                })
                .WithMessage(RoleValidation.PermissionsMustNotContainDuplicates);

            int passwordMaxLength = UserValidation.PasswordMaxLength;

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(passwordMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);
        }
    }
}
