using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.AddUserRole
{
    public record AddUserRoleCommand : Command<AddUserRoleResponse>
    {
        public AddUserRoleCommand(string email, string roleName)
        {
            Email = email;
            RoleName = roleName;
        }

        public string Email { get; }
        public string RoleName { get; }

        public override bool IsValid()
        {
            SetValidationResult(new AddUserRoleValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}