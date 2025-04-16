using SalesSystem.Register.Application.DTOs;

namespace SalesSystem.Register.Application.Commands.Authentication.SignIn
{
    public record SignInUserResponse(string AccessToken, UserTokenDto UserToken, double ExpiresIn);
}