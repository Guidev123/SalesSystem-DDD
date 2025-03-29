using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Payments.Application.Commands.Payments.Confirm
{
    public record ConfirmPaymentCommand : Command<ConfirmPaymentResponse>
    {
        public ConfirmPaymentCommand(string webhookSecret)
        {
            AggregateId = Guid.NewGuid();
            WebhookSecret = webhookSecret;
        }

        public string WebhookSecret { get; } = string.Empty;

        public override bool IsValid()
        {
            SetValidationResult(new ConfirmPaymentValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}
