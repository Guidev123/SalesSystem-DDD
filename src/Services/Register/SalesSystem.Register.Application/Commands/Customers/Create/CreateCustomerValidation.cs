using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerValidation : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidation()
        {
            
        }
    }
}
