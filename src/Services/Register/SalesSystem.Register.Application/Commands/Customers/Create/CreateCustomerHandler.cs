using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Response<CreateCustomerResponse>>
    {
        public Task<Response<CreateCustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
