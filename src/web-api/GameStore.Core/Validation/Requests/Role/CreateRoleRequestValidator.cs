using FluentValidation;
using GameStore.Core.Dto.Requests.Role;
using GameStore.Core.Validation.Details.Role;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Enums;

namespace GameStore.Core.Validation.Requests.Role
{
    public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator()
        {
            RuleFor(x => x.Role)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new RoleDetailsBaseValidator());


            RuleForEach(x => x.Permissions)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);

            RuleFor(x => x.Permissions)
                .Must(list =>
                {
                    IEnumerable<Permission> permissions = list!.ToList();
                    return permissions.Distinct().Count() == permissions.Count();
                })
                .WithMessage(RoleValidation.PermissionsMustNotContainDuplicates);
        }
    }
}
