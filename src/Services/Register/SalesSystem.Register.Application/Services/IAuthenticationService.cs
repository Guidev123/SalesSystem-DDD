using SalesSystem.Register.Application.Commands.Authentication.AddUserRole;
using SalesSystem.Register.Application.Commands.Authentication.CreateRole;
using SalesSystem.Register.Application.Commands.Authentication.Delete;
using SalesSystem.Register.Application.Commands.Authentication.ForgetPassword;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.Commands.Authentication.SignUp;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Services
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