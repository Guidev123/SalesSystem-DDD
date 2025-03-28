using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserHandler(IAuthenticationService authenticationService,
                                            ICustomerRepository customerRepository)
                                          : IRequestHandler<RegisterUserCommand, Response<RegisterUserResponse>>
    {
        public async Task<Response<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
