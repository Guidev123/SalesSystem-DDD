using MediatR;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Queries.Customers.GetById
{
    public sealed class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Response<CustomerDTO>>
    {
        public Task<Response<CustomerDTO>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}