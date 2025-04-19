using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.SignIn
{
    public sealed class SignInUserHandler(IAuthenticationService authenticationService,
                                          INotificator notificator)
                                        : CommandHandler<SignInUserCommand, SignInUserResponse>(notificator)
    {
        public override async Task<Response<SignInUserResponse>> ExecuteAsync(SignInUserCommand request, CancellationToken cancellationToken)
            => !ExecuteValidation(new SignInUserValidation(), request)
            ? Response<SignInUserResponse>.Failure(GetNotifications())
            : await authenticationService.SignInAsync(request).ConfigureAwait(false);
    }
}