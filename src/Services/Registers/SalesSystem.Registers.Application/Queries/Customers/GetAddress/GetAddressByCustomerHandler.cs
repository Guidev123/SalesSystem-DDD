using MidR.Interfaces;
using SalesSystem.Registers.Application.DTOs;
using SalesSystem.Registers.Application.Mappers;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Queries.Customers.GetAddress
{
    public sealed class GetAddressByCustomerHandler(ICustomerRepository customerRepository,
                                                    INotificator notificator)
                                                  : IRequestHandler<GetAddressByCustomerQuery, Response<AddressDto>>
    {
        public async Task<Response<AddressDto>> ExecuteAsync(GetAddressByCustomerQuery request, CancellationToken cancellationToken)
        {
            var customerAddress = await customerRepository.GetAddressByCustomerIdAsync(request.CustomerId);
            if (customerAddress is null)
            {
                notificator.HandleNotification(new("Address not found."));
                return Response<AddressDto>.Failure(notificator.GetNotifications(), code: 404);
            }

            return Response<AddressDto>.Success(customerAddress.MapFromAddress());
        }
    }
}