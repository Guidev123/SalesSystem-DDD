using MidR.Interfaces;
using SalesSystem.Register.Application.Mappers;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Customers.AddAddress
{
    public sealed class AddAddressHandler(ICustomerRepository customerRepository,
                                          INotificator notificator)
                                        : IRequestHandler<AddAddressCommand, Response<AddAddressResponse>>
    {
        public async Task<Response<AddAddressResponse>> ExecuteAsync(AddAddressCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<AddAddressResponse>.Failure(request.GetErrorMessages());

            var customer = await customerRepository.GetByIdAsync(request.CustomerId);
            if (customer is null)
            {
                notificator.HandleNotification(new("Customer not found."));
                return Response<AddAddressResponse>.Failure(notificator.GetNotifications());
            }

            var address = request.MapToAddress();
            customer.SetAddress(address);

            customerRepository.CreateAddress(address);
            if (!await customerRepository.UnitOfWork.CommitAsync())
            {
                notificator.HandleNotification(new("Fail to persist data."));
                return Response<AddAddressResponse>.Failure(notificator.GetNotifications());
            }

            return Response<AddAddressResponse>.Success(new(address.Id), code: 201);
        }
    }
}