using MediatR;
using SalesSystem.Register.Application.Events;
using SalesSystem.Register.Domain.Entities;
using SalesSystem.Register.Domain.Repositories;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerHandler(ICustomerRepository customerRepository,
                                              IMediatorHandler mediator,
                                              INotificator notificator)
                                            : IRequestHandler<CreateCustomerCommand, Response<CreateCustomerResponse>>
    {
        public async Task<Response<CreateCustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await mediator.PublishEventAsync(new CustomerCreationFailedEvent(request.Id, request.Email));
                return Response<CreateCustomerResponse>.Failure(request.GetErrorMessages());
            }

            var customer = new Customer(request.Id, request.Name, request.Email, request.Document, request.BirthDate);

            customerRepository.Create(customer);

            if (!await customerRepository.UnitOfWork.CommitAsync())
            {
                notificator.HandleNotification(new("Fail to persist data."));
                await mediator.PublishEventAsync(new CustomerCreationFailedEvent(request.Id, request.Email));
            }

            return Response<CreateCustomerResponse>.Success(new(customer.Id), code: 201);
        }
    }
}