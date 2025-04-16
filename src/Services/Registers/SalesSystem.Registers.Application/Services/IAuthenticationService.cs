using SalesSystem.Registers.Application.Commands.Authentication.AddUserRole;
using SalesSystem.Registers.Application.Commands.Authentication.CreateRole;
using SalesSystem.Registers.Application.Commands.Authentication.Delete;
using SalesSystem.Registers.Application.Commands.Authentication.ForgetPassword;
using SalesSystem.Registers.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Registers.Application.Commands.Authentication.SignIn;
using SalesSystem.Registers.Application.Commands.Authentication.SignUp;
using SalesSystem.Registers.Application.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Services
{
    public interface IAuthenticationService
    {
        Task<Response<SignUpUserResponse>> RegisterAsync(SignUpUserCommand command);

        Task<Response<SignInUserResponse>> SignInAsync(SignInUserCommand command);

        Task<Response<ResetPasswordUserResponse>> ResetPasswordAsync(ResetPasswordUserCommand command);

        Task<Response<UserDto>> FindByUserEmailAsync(string email);

        Task<Response<IReadOnlyCollection<string>>> FindRolesByUserIdAsync(Guid userId);

        Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command);

        Task<Response<ForgetPasswordUserResponse>> GeneratePasswordResetTokenAsync(ForgetPasswordUserCommand command);

        Task<Response<AddUserRoleResponse>> AddRoleToUserAsync(AddUserRoleCommand command);

        Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command);
    }
}