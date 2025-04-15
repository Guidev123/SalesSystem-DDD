using MidR.Interfaces;
using SalesSystem.Register.Application.Events;
using SalesSystem.Register.Application.Mappers;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerHandler(ICustomerRepository customerRepository,
                                              IMediatorHandler mediator,
                                              INotificator notificator)
                                            : IRequestHandler<CreateCustomerCommand, Response<CreateCustomerResponse>>
    {
        public async Task<Response<CreateCustomerResponse>> ExecuteAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await HandleErrorToCreateCustomer(request);
                return Response<CreateCustomerResponse>.Failure(request.GetErrorMessages());
            }

            var customer = request.MapToCustomer();

            customerRepository.Create(customer);

            if (!await customerRepository.UnitOfWork.CommitAsync())
            {
                notificator.HandleNotification(new("Fail to persist data."));
                await mediator.PublishEventAsync(new CustomerCreationFailedEvent(request.Id, request.Email));
            }

            return Response<CreateCustomerResponse>.Success(new(customer.Id), code: 201);
        }

        private async Task HandleErrorToCreateCustomer(CreateCustomerCommand command)
        {
            await mediator.PublishEventAsync(new CustomerCreationFailedEvent(command.Id, command.Email));
            foreach (var item in command.GetErrorMessages())
            {
                notificator.HandleNotification(new(item));
            }
        }
    }
}