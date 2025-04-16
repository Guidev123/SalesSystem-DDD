using MidR.Interfaces;
using SalesSystem.Registers.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Authentication.SignIn
{
    public sealed class SignInUserHandler(IAuthenticationService authenticationService)
                                        : IRequestHandler<SignInUserCommand, Response<SignInUserResponse>>
    {
        public async Task<Response<SignInUserResponse>> ExecuteAsync(SignInUserCommand request, CancellationToken cancellationToken)
            => !request.IsValid()
            ? Response<SignInUserResponse>.Failure(request.GetErrorMessages())
            : await authenticationService.SignInAsync(request).ConfigureAwait(false);
    }
}