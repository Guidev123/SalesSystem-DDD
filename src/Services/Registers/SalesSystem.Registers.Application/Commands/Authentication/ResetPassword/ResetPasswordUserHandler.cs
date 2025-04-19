using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.ResetPassword
{
    public sealed class ResetPasswordUserHandler(IAuthenticationService authenticationService,
                                                 INotificator notificator)
                                               : CommandHandler<ResetPasswordUserCommand, ResetPasswordUserResponse>(notificator)
    {
        public override async Task<Response<ResetPasswordUserResponse>> ExecuteAsync(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            return !ExecuteValidation(new ResetPasswordUserValidation(), request)
                ? Response<ResetPasswordUserResponse>.Failure(GetNotifications())
                : await authenticationService.ResetPasswordAsync(request);
        }
    }
}