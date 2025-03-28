using Microsoft.AspNetCore.Identity;
using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Services;
using SalesSystem.Register.Infrastructure.Mappers;
using SalesSystem.Register.Infrastructure.Models;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Infrastructure.Services
{
    public sealed class AuthenticationService(SignInManager<User> signInManager,
                                              INotificator notificator,
                                              UserManager<User> userManager)
                                            : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly INotificator _notificator = notificator;

        public Task<Response<UserDTO>> FindByUserEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command)
        {
            var user = command.MapToUser();

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    _notificator.HandleNotification(new(item.Description));
                }

                return Response<RegisterUserResponse>.Failure(_notificator.GetNotifications());
            }

            var userIdentity = await _userManager.FindByEmailAsync(command.Email);
            if(userIdentity is null)
            {
                _notificator.HandleNotification(new("Fail to create user."));
                return Response<RegisterUserResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<RegisterUserResponse>.Success(new(Guid.Parse(userIdentity.Id)), code: 201);
        }

        public Task<Response<SignInUserResponse>> SignInAsync(SignInUserCommand command)
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


        public Task<Response<ResetPasswordUserResponse>> ResetPasswordAsync(ResetPasswordUserCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
