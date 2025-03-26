using FluentValidation;

namespace SalesSystem.Payments.Application.Commands.Payments.Checkout
{
    public sealed class CheckoutPaymentValidation : AbstractValidator<CheckoutPaymentCommand>
    {
        public CheckoutPaymentValidation()
        {
            
        }
    }
}
