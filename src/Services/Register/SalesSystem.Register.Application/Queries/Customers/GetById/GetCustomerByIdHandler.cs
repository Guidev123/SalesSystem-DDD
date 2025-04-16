using MidR.Interfaces;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Mappers;
using SalesSystem.Register.Application.Services;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Queries.Customers.GetById
{
    public sealed class GetCustomerByIdHandler(ICustomerRepository customerRepository,
                                               INotificator notificator,
                                               IAuthenticationService authenticationService)
                                             : IRequestHandler<GetCustomerByIdQuery, Response<CustomerDto>>
    {
        public async Task<Response<CustomerDto>> ExecuteAsync(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {   
            var customer = await customerRepository.GetCustomerAddressByIdAsync(request.CustomerId);
            if(customer is null)
            {
                notificator.HandleNotification(new("Customer not found."));
                return Response<CustomerDto>.Failure(notificator.GetNotifications(), code: 404);
            }

            var roles = await authenticationService.FindRolesByUserIdAsync(customer.Id);

            return Response<CustomerDto>.Success(customer.MapFromCustomer(roles.Data));
        }
    }
}