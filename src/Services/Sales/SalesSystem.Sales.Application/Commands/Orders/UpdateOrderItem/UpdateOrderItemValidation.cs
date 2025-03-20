using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem
{
    public sealed class UpdateOrderItemValidation : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id cannot be empty.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Order Id cannot be empty.");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Product Id cannot be empty.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Quantity)
            .LessThan(15)
            .WithMessage("Quantity must be less than 15.");
        }
    }
}