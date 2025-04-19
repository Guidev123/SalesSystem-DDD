using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.CreateRole
{
    public sealed class CreateRoleHandler(IAuthenticationService authenticationService,
                                          INotificator notificator)
                                        : CommandHandler<CreateRoleCommand, CreateRoleResponse>(notificator)
    {
        public override async Task<Response<CreateRoleResponse>> ExecuteAsync(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new CreateRoleValidation(), request))
                return Response<CreateRoleResponse>.Failure(GetNotifications());

            return await authenticationService.CreateRoleAsync(request);
        }
    }
}