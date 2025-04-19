using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.Delete
{
    public sealed class DeleteUserHandler(IAuthenticationService authenticationService,
                                          INotificator notificator)
                                        : CommandHandler<DeleteUserCommand, DeleteUserResponse>(notificator)
    {
        public override async Task<Response<DeleteUserResponse>> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new DeleteUserValidation(), request))
                return Response<DeleteUserResponse>.Failure(GetNotifications());

            return await authenticationService.DeleteAsync(request);
        }
    }
}