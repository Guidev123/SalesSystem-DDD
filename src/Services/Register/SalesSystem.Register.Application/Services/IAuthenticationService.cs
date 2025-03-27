using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Services
{
    public interface IAuthenticationService
    {
        Task<Response<UserDTO>> RegisterAsync(RegisterUserCommand command);
        Task<Response<UserDTO>> SignInAsync(SignInUserCommand command);
        Task<Response<UserDTO>> ResetPasswordAsync(ResetPasswordUserCommand command);
        Task<Response<UserDTO>> FindByUserEmailAsync(string email);
        Task<Response<UserDTO>> CheckPasswordAsync(UserDTO userDTO, string password);
        Task<Response<UserDTO>> DeleteAsync(UserDTO userDTO);
        Task<Response<string>> GeneratePasswordResetTokenAsync(UserDTO userDTO);
    }
}
