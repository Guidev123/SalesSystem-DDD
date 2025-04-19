using SalesSystem.Registers.Application.Mappers;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Customers.AddAddress
{
    public sealed class AddAddressHandler(ICustomerRepository customerRepository,
                                          INotificator notificator)
                                        : CommandHandler<AddAddressCommand, AddAddressResponse>(notificator)
    {
        public override async Task<Response<AddAddressResponse>> ExecuteAsync(AddAddressCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new AddAddressValidation(), request))
                return Response<AddAddressResponse>.Failure(GetNotifications());

            var customer = await customerRepository.GetByIdAsync(request.CustomerId);
            if (customer is null)
            {
                Notify("Customer not found.");
                return Response<AddAddressResponse>.Failure(GetNotifications());
            }

            var address = request.MapToAddress();
            customer.SetAddress(address);

            customerRepository.CreateAddress(address);
            if (!await customerRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<AddAddressResponse>.Failure(GetNotifications());
            }

            return Response<AddAddressResponse>.Success(new(address.Id), code: 201);
        }
    }
}