using MidR.Interfaces;
using SalesSystem.Register.Application.Events;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserHandler(IAuthenticationService authenticationService,
                                                INotificator notificator,
                                                IMediatorHandler mediator)
                                              : IRequestHandler<RegisterUserCommand, Response<RegisterUserResponse>>
    {
        public async Task<Response<RegisterUserResponse>> ExecuteAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<RegisterUserResponse>.Failure(request.GetErrorMessages());

            var result = await authenticationService.RegisterAsync(request).ConfigureAwait(false);
            if (!result.IsSuccess || result.Data is null)
            {
                notificator.HandleNotification(new("User not found."));
                return Response<RegisterUserResponse>.Failure(notificator.GetNotifications());
            }

            await mediator.PublishEventAsync(new UserCreatedEvent(result.Data.Id, request.Name, request.Email, request.Document, request.BirthDate));
            return notificator.HasNotifications()
                ? Response<RegisterUserResponse>.Failure(notificator.GetNotifications())
                : Response<RegisterUserResponse>.Success(new(result.Data.Id), code: 201);
        }
    }
}