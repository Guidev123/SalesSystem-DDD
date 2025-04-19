using SalesSystem.SharedKernel.Abstractions;

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
    }
}