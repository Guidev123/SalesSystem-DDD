using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.SignIn
{
    public sealed class SignInUserHandler(IAuthenticationService authenticationService)
                                        : IRequestHandler<SignInUserCommand, Response<SignInUserResponse>>
    {
        public async Task<Response<SignInUserResponse>> Handle(SignInUserCommand request, CancellationToken cancellationToken)
            => !request.IsValid()
            ? Response<SignInUserResponse>.Failure(request.GetErrorMessages())
            : await authenticationService.SignInAsync(request).ConfigureAwait(false);
    }
}