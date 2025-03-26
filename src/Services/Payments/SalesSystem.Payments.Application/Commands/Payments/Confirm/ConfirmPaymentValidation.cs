using FluentValidation;

namespace SalesSystem.Payments.Application.Commands.Payments.Confirm
{
    public sealed class ConfirmPaymentValidation : AbstractValidator<ConfirmPaymentCommand>
    {
        public ConfirmPaymentValidation()
        {
            
        }
    }
}
