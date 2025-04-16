using MidR.Interfaces;
using SalesSystem.Register.Application.Events;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.SignUp
{
    public sealed class SignUpUserHandler(IAuthenticationService authenticationService,
                                                INotificator notificator,
                                                IMediatorHandler mediator)
                                              : IRequestHandler<SignUpUserCommand, Response<SignUpUserResponse>>
    {
        public async Task<Response<SignUpUserResponse>> ExecuteAsync(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<SignUpUserResponse>.Failure(request.GetErrorMessages());

            var result = await authenticationService.RegisterAsync(request).ConfigureAwait(false);
            if (!result.IsSuccess || result.Data is null)
            {
                notificator.HandleNotification(new("User not found."));
                return Response<SignUpUserResponse>.Failure(notificator.GetNotifications());
            }

            await mediator.PublishEventAsync(new UserCreatedEvent(result.Data.Id, request.Name, request.Email, request.Document, request.BirthDate));
            return notificator.HasNotifications()
                ? Response<SignUpUserResponse>.Failure(notificator.GetNotifications())
                : Response<SignUpUserResponse>.Success(new(result.Data.Id), code: 201);
        }
    }
}