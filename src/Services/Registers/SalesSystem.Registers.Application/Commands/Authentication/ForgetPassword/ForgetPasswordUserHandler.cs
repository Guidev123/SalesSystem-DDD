using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.ForgetPassword
{
    public sealed class ForgetPasswordUserHandler(IAuthenticationService authenticationService,
                                                  INotificator notificator)
                                                : CommandHandler<ForgetPasswordUserCommand, ForgetPasswordUserResponse>(notificator)
    {
        public override async Task<Response<ForgetPasswordUserResponse>> ExecuteAsync(ForgetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            return !ExecuteValidation(new ForgetPasswordUserValidation(), request)
                ? Response<ForgetPasswordUserResponse>.Failure(GetNotifications())
                : await authenticationService.GeneratePasswordResetTokenAsync(request);
        }
    }
}