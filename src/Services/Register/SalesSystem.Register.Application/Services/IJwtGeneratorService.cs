using SalesSystem.Register.Application.Commands.Authentication.SignIn;

namespace SalesSystem.Register.Application.Services
{
    public interface IJwtGeneratorService
    {
        Task<SignInUserResponse> JwtGenerator(string email);
    }
}
