using FluentValidation;

namespace SalesSystem.Registers.Application.Commands.Authentication.AddUserRole
{
    public sealed class AddUserRoleValidation : AbstractValidator<AddUserRoleCommand>
    {
        public AddUserRoleValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid Email address.");

            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage("Role name is required.");
        }
    }
}