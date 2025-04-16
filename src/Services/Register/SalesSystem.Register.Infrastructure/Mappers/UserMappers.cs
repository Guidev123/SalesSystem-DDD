using SalesSystem.Register.Application.Commands.Authentication.SignUp;
using SalesSystem.Register.Infrastructure.Models;

namespace SalesSystem.Register.Infrastructure.Mappers
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