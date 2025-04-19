using SalesSystem.Registers.Application.Events;
using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.SignUp
{
    public sealed class SignUpUserHandler(IAuthenticationService authenticationService,
                                                INotificator notificator,
                                                IMediatorHandler mediator)
                                              : CommandHandler<SignUpUserCommand, SignUpUserResponse>(notificator)
    {
        public override async Task<Response<SignUpUserResponse>> ExecuteAsync(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new SignUpUserValidation(), request))
                return Response<SignUpUserResponse>.Failure(GetNotifications());

            var result = await authenticationService.RegisterAsync(request).ConfigureAwait(false);
            if (!result.IsSuccess || result.Data is null)
            {
                Notify("User not found.");
                return Response<SignUpUserResponse>.Failure(GetNotifications());
            }

            await mediator.PublishEventAsync(new UserCreatedEvent(result.Data.Id, request.Name, request.Email, request.Document, request.BirthDate));
            return OperationIsValid()
                ? Response<SignUpUserResponse>.Success(new(result.Data.Id), code: 201)
                : Response<SignUpUserResponse>.Failure(GetNotifications());
        }
    }
}