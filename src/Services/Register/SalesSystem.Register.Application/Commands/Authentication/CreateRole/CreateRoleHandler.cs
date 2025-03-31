using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.CreateRole
{
    public sealed class CreateRoleHandler(IAuthenticationService authenticationService)
                                        : IRequestHandler<CreateRoleCommand, Response<CreateRoleResponse>>
    {
        public async Task<Response<CreateRoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return Response<CreateRoleResponse>.Failure(request.GetErrorMessages());

            return await authenticationService.CreateRoleAsync(request);
        }
    }
}
