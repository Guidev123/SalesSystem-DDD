using SalesSystem.Registers.Application.Commands.Authentication.SignIn;

namespace SalesSystem.Registers.Application.Services
{
    public interface IJwtGeneratorService
    {
        Task<SignInUserResponse> JwtGenerator(string email);
    }
}