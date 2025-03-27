using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Response<RegisterUserResponse>>
    {
        public async Task<Response<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
