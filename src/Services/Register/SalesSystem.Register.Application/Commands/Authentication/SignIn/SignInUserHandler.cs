using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.SignIn
{
    public sealed class SignInUserHandler : IRequestHandler<SignInUserCommand, Response<SignInUserResponse>>
    {
        public Task<Response<SignInUserResponse>> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
