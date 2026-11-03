using FluentValidation;
using GameStore.Core.Dto.Details.Role;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Details.Role
{
    public class RoleDetailsBaseValidator : AbstractValidator<RoleDetailsBase>
    {
        public RoleDetailsBaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            int nameMaxLength = RoleValidation.NameMaxLength;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(nameMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);
        }
    }
}
