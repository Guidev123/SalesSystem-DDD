using MidR.Interfaces;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.AddUserRole
{
    public sealed class AddUserRoleHandler(IAuthenticationService authenticationService)
                                         : IRequestHandler<AddUserRoleCommand, Response<AddUserRoleResponse>>
    {
        public async Task<Response<AddUserRoleResponse>> ExecuteAsync(AddUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<AddUserRoleResponse>.Failure(request.GetErrorMessages());

            return await authenticationService.AddRoleToUserAsync(request);
        }
    }
}