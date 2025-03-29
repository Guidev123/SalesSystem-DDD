using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Authentication.ResetPassword
{
    public sealed class ResetPasswordUserHandler : IRequestHandler<ResetPasswordUserCommand, Response<ResetPasswordUserResponse>>
    {
        public Task<Response<ResetPasswordUserResponse>> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}