using FluentValidation;

namespace SalesSystem.Payments.Application.Commands.Payments.Checkout
{
    public sealed class CheckoutPaymentValidation : AbstractValidator<CheckoutPaymentCommand>
    {
        public CheckoutPaymentValidation()
        {
            RuleFor(order => order.OrderId)
                .NotEmpty().WithMessage("Order ID cannot be empty.");

            RuleFor(order => order.Value)
                .GreaterThan(0).WithMessage("Order value must be greater than zero.");

            RuleFor(order => order.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(order => order.OrderCode)
                .NotEmpty().WithMessage("Order code is required.");

            RuleFor(order => order.CustomerId)
                .NotEmpty().WithMessage("Customer ID cannot be empty.");

            RuleFor(order => order.Products)
                .NotEmpty().WithMessage("At least one product must be included in the order.");
        }
    }
}