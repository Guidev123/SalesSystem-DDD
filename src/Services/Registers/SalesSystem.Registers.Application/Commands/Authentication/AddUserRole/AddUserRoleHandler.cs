using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.AddUserRole
{
    public sealed class AddUserRoleHandler(IAuthenticationService authenticationService,
                                           INotificator notificator)
                                         : CommandHandler<AddUserRoleCommand, AddUserRoleResponse>(notificator)
    {
        public override async Task<Response<AddUserRoleResponse>> ExecuteAsync(AddUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new AddUserRoleValidation(), request))
                return Response<AddUserRoleResponse>.Failure(GetNotifications());

            return await authenticationService.AddRoleToUserAsync(request);
        }
    }
}