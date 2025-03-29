using MediatR;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Mappers;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Queries.Customers.GetAddress
{
    public sealed class GetAddressByCustomerHandler(ICustomerRepository customerRepository,
                                                    INotificator notificator)
                                                  : IRequestHandler<GetAddressByCustomerQuery, Response<AddressDTO>>
    {
        public async Task<Response<AddressDTO>> Handle(GetAddressByCustomerQuery request, CancellationToken cancellationToken)
        {
            var customerAddress = await customerRepository.GetAddressByCustomerIdAsync(request.CustomerId);
            if (customerAddress is null)
            {
                notificator.HandleNotification(new("Address not found."));
                return Response<AddressDTO>.Failure(notificator.GetNotifications(), code: 404);
            }

            return Response<AddressDTO>.Success(customerAddress.MapFromAddress());
        }
    }
}
