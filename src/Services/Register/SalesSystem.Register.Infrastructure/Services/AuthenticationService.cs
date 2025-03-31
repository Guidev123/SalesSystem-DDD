using Microsoft.AspNetCore.Identity;
using SalesSystem.Email;
using SalesSystem.Email.Models;
using SalesSystem.Register.Application.Commands.Authentication.AddUserRole;
using SalesSystem.Register.Application.Commands.Authentication.CreateRole;
using SalesSystem.Register.Application.Commands.Authentication.Delete;
using SalesSystem.Register.Application.Commands.Authentication.ForgetPassword;
using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Services;
using SalesSystem.Register.Infrastructure.Mappers;
using SalesSystem.Register.Infrastructure.Models;
using SalesSystem.SharedKernel.Enums;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Infrastructure.Services
{
    public sealed class AuthenticationService(SignInManager<User> signInManager,
                                              INotificator notificator,
                                              UserManager<User> userManager,
                                              RoleManager<IdentityRole> roleManager,
                                              IEmailService emailService,
                                              IJwtGeneratorService jwtGeneratorService)
                                            : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IJwtGeneratorService _jwtGeneratorService = jwtGeneratorService;
        private readonly INotificator _notificator = notificator;
        private readonly IEmailService _emailService = emailService;

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
        public async Task<Response<ResetPasswordUserResponse>> ResetPasswordAsync(ResetPasswordUserCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                _notificator.HandleNotification(new("User not found."));
                return Response<ResetPasswordUserResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var result = await _userManager.ResetPasswordAsync(user, command.Token, command.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    _notificator.HandleNotification(new(item.Description));
                }

                return Response<ResetPasswordUserResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<ResetPasswordUserResponse>.Success(default, code: 204);
        }

        public async Task<Response<ForgetPasswordUserResponse>> GeneratePasswordResetTokenAsync(ForgetPasswordUserCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                _notificator.HandleNotification(new("User not found."));
                return Response<ForgetPasswordUserResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var message = new EmailMessage(command.Email, "Token to reset password.", token);
            await _emailService.SendAsync(message);

            return Response<ForgetPasswordUserResponse>.Success(default, code: 204);
        }

        public async Task<Response<AddUserRoleResponse>> AddRoleToUserAsync(AddUserRoleCommand command)
        {
            var roleIsValid = await RoleIsValidAsync(command.RoleName);
            if (!roleIsValid)
            {
                _notificator.HandleNotification(new("Invalid role name."));
                return Response<AddUserRoleResponse>.Failure(_notificator.GetNotifications());
            }

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                _notificator.HandleNotification(new("User not found."));
                return Response<AddUserRoleResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var result = await _userManager.AddToRoleAsync(user, command.RoleName);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    _notificator.HandleNotification(new(item.Description));
                }
                return Response<AddUserRoleResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<AddUserRoleResponse>.Success(default, code: 204);
        }

        public async Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command)
        {
            if (!RoleIsInEnum(command.RoleName))
            {
                _notificator.HandleNotification(new("Invalid role name. Must be a valid role from enum UserRoles."));
                return Response<CreateRoleResponse>.Failure(_notificator.GetNotifications());
            }

            if (await _roleManager.RoleExistsAsync(command.RoleName))
            {
                _notificator.HandleNotification(new($"Role '{command.RoleName}' already exists."));
                return Response<CreateRoleResponse>.Failure(_notificator.GetNotifications());
            }

            var role = new IdentityRole(command.RoleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded || role.Name is null)
            {
                foreach (var error in result.Errors)
                {
                    _notificator.HandleNotification(new(error.Description));
                }
                return Response<CreateRoleResponse>.Failure(_notificator.GetNotifications());
            }

            var roleResult = new CreateRoleResponse(Guid.Parse(role.Id));
            return Response<CreateRoleResponse>.Success(roleResult, code: 201);
        }

        private async Task<bool> RoleIsValidAsync(string roleName)
        {
            var roleIsValid = RoleIsInEnum(roleName);

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            return roleIsValid && roleExists;
        }

        private static bool RoleIsInEnum(string roleName) 
            => Enum.GetNames<EUserRoles>().Any(name => name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }
}