using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.CreateRole
{
    public record CreateRoleCommand : Command<CreateRoleResponse>
    {
        public CreateRoleCommand(string roleName) => RoleName = roleName;

        public string RoleName { get; } = string.Empty;

        public override bool IsValid()
        {
            SetValidationResult(new CreateRoleValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}