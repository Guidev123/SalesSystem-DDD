using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.ResetPassword
{
    public sealed class ResetPasswordUserHandler(IAuthenticationService authenticationService)
                                               : IRequestHandler<ResetPasswordUserCommand, Response<ResetPasswordUserResponse>>
    {
        public async Task<Response<ResetPasswordUserResponse>> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<ResetPasswordUserResponse>.Failure(request.GetErrorMessages());

            return await authenticationService.ResetPasswordAsync(request);
        }
    }
}