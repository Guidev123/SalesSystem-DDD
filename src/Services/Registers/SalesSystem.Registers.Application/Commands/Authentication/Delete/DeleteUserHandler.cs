using SalesSystem.Registers.Application.Events;
using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.Delete
{
    public sealed class DeleteUserHandler(IAuthenticationService authenticationService,
                                          INotificator notificator,
                                          IMediatorHandler mediatorHandler)
                                        : CommandHandler<DeleteUserCommand, DeleteUserResponse>(notificator)
    {
        public override async Task<Response<DeleteUserResponse>> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new DeleteUserValidation(), request))
                return Response<DeleteUserResponse>.Failure(GetNotifications());

            var result = await authenticationService.DeleteAsync(request);
            if (!result.IsSuccess || result.Data is null)
                return Response<DeleteUserResponse>.Failure(GetNotifications());

            await mediatorHandler.PublishEventAsync(new UserDeletedEvent(result.Data.Id));

            return OperationIsValid()
                ? Response<DeleteUserResponse>.Success(new(result.Data.Id), code: 204)
                : Response<DeleteUserResponse>.Failure(GetNotifications());
        }
    }
}