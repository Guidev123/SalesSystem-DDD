using MediatR;
using SalesSystem.Register.Application.Services;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserHandler(IAuthenticationService authenticationService,
                                            IMediatorHandler mediator)
                                          : IRequestHandler<RegisterUserCommand, Response<RegisterUserResponse>>
    {
        public async Task<Response<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                Response<RegisterUserResponse>.Failure(request.GetErrorMessages());

           var result =  await authenticationService.RegisterAsync(request).ConfigureAwait(false);

            va

            return result;
        }
    }
}
