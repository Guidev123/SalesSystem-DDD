using FluentValidation;

namespace SalesSystem.Payments.Application.Commands.Payments.Confirm
{
    public sealed class ConfirmPaymentValidation : AbstractValidator<ConfirmPaymentCommand>
    {
        public ConfirmPaymentValidation()
        {
            RuleFor(x => x.WebhookSecret).NotEmpty().WithMessage("Webhook must be not empty.");
        }
    }
}