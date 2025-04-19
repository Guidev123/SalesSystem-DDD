using SalesSystem.Registers.Application.Events;
using SalesSystem.Registers.Application.Mappers;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerHandler(ICustomerRepository customerRepository,
                                              IMediatorHandler mediator,
                                              INotificator notificator)
                                            : CommandHandler<CreateCustomerCommand, CreateCustomerResponse>(notificator)
    {
        public override async Task<Response<CreateCustomerResponse>> ExecuteAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new CreateCustomerValidation(), request))
            {
                await HandleErrorToCreateCustomer(request);
                return Response<CreateCustomerResponse>.Failure(GetNotifications());
            }

            var customer = request.MapToCustomer();

            customerRepository.Create(customer);

            if (!await customerRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                await mediator.PublishEventAsync(new CustomerCreationFailedEvent(request.Id, request.Email));
            }

            return Response<CreateCustomerResponse>.Success(new(customer.Id), code: 201);
        }

        private async Task HandleErrorToCreateCustomer(CreateCustomerCommand command)
            => await mediator.PublishEventAsync(new CustomerCreationFailedEvent(command.Id, command.Email));
    }
}