using MidR.Interfaces;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Mappers;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Queries.Customers.GetAddress
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