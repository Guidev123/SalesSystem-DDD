using SalesSystem.Registers.Application.Commands.Authentication.SignUp;
using SalesSystem.Registers.Infrastructure.Models;

namespace SalesSystem.Registers.Infrastructure.Mappers
{
    public static class UserMappers
    {
        public static User MapToUser(this SignUpUserCommand command) => new()
        {
            UserName = command.Email,
            Email = command.Email,
            EmailConfirmed = true
        };
    }
}