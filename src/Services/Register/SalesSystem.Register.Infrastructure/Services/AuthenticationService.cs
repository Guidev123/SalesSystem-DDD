using Microsoft.AspNetCore.Identity;
using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Services;
using SalesSystem.Register.Infrastructure.Models;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Infrastructure.Services
{
    public sealed class AuthenticationService(SignInManager<User> signInManager,
                                              UserManager<User> userManager)
                                            : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        public Task<Response<UserDTO>> FindByUserEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserDTO>> RegisterAsync(RegisterUserCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserDTO>> SignInAsync(SignInUserCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserDTO>> CheckPasswordAsync(UserDTO userDTO, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserDTO>> DeleteAsync(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }


        public Task<Response<string>> GeneratePasswordResetTokenAsync(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }


        public Task<Response<UserDTO>> ResetPasswordAsync(ResetPasswordUserCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
