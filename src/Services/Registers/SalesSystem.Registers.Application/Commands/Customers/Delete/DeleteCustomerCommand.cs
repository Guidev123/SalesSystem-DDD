using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Registers.Application.Commands.Customers.Delete
{
    public record DeleteCustomerCommand(Guid UserId) : Command<DeleteCustomerResponse>;
}
