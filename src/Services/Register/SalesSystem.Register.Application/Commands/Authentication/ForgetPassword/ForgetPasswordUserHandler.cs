using MidR.Interfaces;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.ForgetPassword
{
    public sealed class ForgetPasswordUserHandler(IAuthenticationService authenticationService)
                                                : IRequestHandler<ForgetPasswordUserCommand, Response<ForgetPasswordUserResponse>>
    {
        public async Task<Response<ForgetPasswordUserResponse>> ExecuteAsync(ForgetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<ForgetPasswordUserResponse>.Failure(request.GetErrorMessages());

            return await authenticationService.GeneratePasswordResetTokenAsync(request);
        }
    }
}