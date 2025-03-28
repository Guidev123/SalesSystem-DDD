using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserHandler(IAuthenticationService authenticationService)
                                          : IRequestHandler<RegisterUserCommand, Response<RegisterUserResponse>>
    {
        public async Task<Response<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            => !request.IsValid()
                ? Response<RegisterUserResponse>.Failure(request.GetErrorMessages())
                : await authenticationService.RegisterAsync(request).ConfigureAwait(false);
    }
}
