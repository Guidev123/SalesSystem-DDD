using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem
{
    public sealed class RemoveOrderItemValidation : AbstractValidator<RemoveOrderItemCommand>
    {
        public RemoveOrderItemValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id cannot be empty.");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Product Id cannot be empty.");
        }
    }
}