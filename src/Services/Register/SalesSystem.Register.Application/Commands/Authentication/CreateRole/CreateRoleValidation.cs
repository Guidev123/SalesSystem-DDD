using FluentValidation;
using SalesSystem.SharedKernel.Enums;

namespace SalesSystem.Register.Application.Commands.Authentication.CreateRole
{
    public sealed class CreateRoleValidation : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidation()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage("Role name is required.")
                .Must(RoleIsInEnum)
                .WithMessage("Invalid role.");
        }

        private static bool RoleIsInEnum(string roleName)
            => Enum.GetNames<EUserRoles>().Any(name => name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }
}
