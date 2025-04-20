using FluentValidation;

namespace SalesSystem.Registers.Application.Commands.Customers.Delete
{
    public sealed class DeleteCustomerValidation : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerValidation()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("UserId is required.");
        }
    }
}
