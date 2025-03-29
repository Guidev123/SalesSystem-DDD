using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.Delete
{
    public sealed class DeleteUserHandler(IAuthenticationService authenticationService)
                                        : IRequestHandler<DeleteUserCommand, Response<DeleteUserResponse>>
    {
        public async Task<Response<DeleteUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<DeleteUserResponse>.Failure(request.GetErrorMessages());

            return await authenticationService.DeleteAsync(request);
        }
    }
}