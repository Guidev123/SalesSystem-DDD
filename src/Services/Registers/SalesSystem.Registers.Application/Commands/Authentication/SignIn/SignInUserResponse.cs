using SalesSystem.Registers.Application.DTOs;

namespace SalesSystem.Registers.Application.Commands.Authentication.SignIn
{
    public record SignInUserResponse(string AccessToken, UserTokenDto UserToken, double ExpiresIn);
}