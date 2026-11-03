using FluentValidation;
using GameStore.Core.Dto.Details.Role;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Role
{
    public class RoleDetailsWithIdValidator : AbstractValidator<RoleDetailsWithId>
    {
        public RoleDetailsWithIdValidator()
        {
            Include(new RoleDetailsBaseValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}
