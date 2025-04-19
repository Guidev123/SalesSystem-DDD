using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Registers.Application.Commands.Authentication.AddUserRole
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
    }
}