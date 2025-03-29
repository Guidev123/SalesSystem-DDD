using Microsoft.AspNetCore.Identity;
using SalesSystem.Register.Application.Commands.Authentication.Delete;
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
                                              UserManager<User> userManager,
                                              IJwtGeneratorService jwtGeneratorService)
                                            : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IJwtGeneratorService _jwtGeneratorService = jwtGeneratorService;
        private readonly INotificator _notificator = notificator;

        public async Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command)
        {
            var user = command.MapToUser();

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    _notificator.HandleNotification(new(item.Description));
                }

                return Response<RegisterUserResponse>.Failure(_notificator.GetNotifications());
            }

            var userIdentity = await FindByUserEmailAsync(command.Email);
            if (!userIdentity.IsSuccess || userIdentity.Data is null)
            {
                _notificator.HandleNotification(new("Fail to create user."));
                return Response<RegisterUserResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<RegisterUserResponse>.Success(new(userIdentity.Data.UserId), code: 201);
        }

        public async Task<Response<SignInUserResponse>> SignInAsync(SignInUserCommand command)
        {
            var result = await _signInManager.PasswordSignInAsync(command.Email, command.Password, false, true);
            if (!result.Succeeded)
            {
                _notificator.HandleNotification(new("Invalid user credentials."));
                return Response<SignInUserResponse>.Failure(_notificator.GetNotifications());
            }

            if (result.IsLockedOut)
            {
                _notificator.HandleNotification(new("You cannot SignIn now, try later."));
                return Response<SignInUserResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<SignInUserResponse>.Success(await _jwtGeneratorService.JwtGenerator(command.Email));
        }

        public async Task<Response<UserDTO>> FindByUserEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user is null)
            {
                _notificator.HandleNotification(new("User not found."));
                return Response<UserDTO>.Failure(_notificator.GetNotifications());
            }

            return Response<UserDTO>.Success(new(Guid.Parse(user.Id), user.Email ?? string.Empty));
        }

        public Task<Response<UserDTO>> CheckPasswordAsync(UserDTO userDTO, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                _notificator.HandleNotification(new("User not found."));
                return Response<DeleteUserResponse>.Failure(_notificator.GetNotifications());
            }

            var delete = await _userManager.DeleteAsync(user);
            if (!delete.Succeeded)
            {
                foreach (var item in delete.Errors)
                {
                    _notificator.HandleNotification(new(item.Description));
                }

                return Response<DeleteUserResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<DeleteUserResponse>.Success(new(Guid.Parse(user.Id)), code: 204);
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