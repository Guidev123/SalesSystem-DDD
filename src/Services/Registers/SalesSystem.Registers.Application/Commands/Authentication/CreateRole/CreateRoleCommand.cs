using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Registers.Application.Commands.Authentication.CreateRole
{
    public record CreateRoleCommand : Command<CreateRoleResponse>
    {
        public CreateRoleCommand(string roleName) => RoleName = roleName;

        public string RoleName { get; } = string.Empty;
    }
}