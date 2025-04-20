using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Registers.Application.Commands.Customers.Delete
{
    public sealed class DeleteCustomerHandler(INotificator notificator,
                                                     ICustomerRepository customerRepository)
                      : CommandHandler<DeleteCustomerCommand, DeleteCustomerResponse>(notificator)
    {
        public async override Task<Response<DeleteCustomerResponse>> ExecuteAsync(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new DeleteCustomerValidation(), request))
                return Response<DeleteCustomerResponse>.Failure(GetNotifications());

            var customer = await customerRepository.GetByIdAsync(request.UserId);
            if(customer is null)
            {
                Notify("Customer not found.");
                return Response<DeleteCustomerResponse>.Failure(GetNotifications());
            }

            customer.SetDeleted();
            customerRepository.Update(customer);

            if (!await customerRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<DeleteCustomerResponse>.Failure(GetNotifications());
            }

            return Response<DeleteCustomerResponse>.Success(default, code: 204);
        }
    }
}
