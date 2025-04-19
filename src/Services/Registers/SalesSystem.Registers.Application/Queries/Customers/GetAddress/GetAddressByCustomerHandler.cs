using SalesSystem.Registers.Application.DTOs;
using SalesSystem.Registers.Application.Mappers;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Queries.Customers.GetAddress
{
    public sealed class GetAddressByCustomerHandler(ICustomerRepository customerRepository,
                                                    INotificator notificator)
                                                  : QueryHandler<GetAddressByCustomerQuery, AddressDto>(notificator)
    {
        public override async Task<Response<AddressDto>> ExecuteAsync(GetAddressByCustomerQuery request, CancellationToken cancellationToken)
        {
            var customerAddress = await customerRepository.GetAddressByCustomerIdAsync(request.CustomerId);
            if (customerAddress is null)
            {
                Notify("Address not found.");
                return Response<AddressDto>.Failure(GetNotifications(), code: 404);
            }

            return Response<AddressDto>.Success(customerAddress.MapFromAddress());
        }
    }
}