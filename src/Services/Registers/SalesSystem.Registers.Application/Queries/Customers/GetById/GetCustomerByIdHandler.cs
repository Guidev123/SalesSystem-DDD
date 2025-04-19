using SalesSystem.Registers.Application.DTOs;
using SalesSystem.Registers.Application.Mappers;
using SalesSystem.Registers.Application.Services;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Queries.Customers.GetById
{
    public sealed class GetCustomerByIdHandler(ICustomerRepository customerRepository,
                                               INotificator notificator,
                                               IAuthenticationService authenticationService)
                                             : QueryHandler<GetCustomerByIdQuery, CustomerDto>(notificator)
    {
        public override async Task<Response<CustomerDto>> ExecuteAsync(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetCustomerAddressByIdAsync(request.CustomerId);
            if (customer is null)
            {
                Notify("Customer not found.");
                return Response<CustomerDto>.Failure(GetNotifications(), code: 404);
            }

            var roles = await authenticationService.FindRolesByUserIdAsync(customer.Id);

            return Response<CustomerDto>.Success(customer.MapFromCustomer(roles.Data));
        }
    }
}