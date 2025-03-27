using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public record CreateCustomerCommand : Command<CreateCustomerResponse>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
