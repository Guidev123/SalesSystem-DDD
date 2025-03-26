using SalesSystem.Payments.Application.DTOs;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Payments.Application.Commands.Payments.Checkout
{
    public record CheckoutPaymentCommand : Command<CheckoutPaymentResponse>
    {
        public CheckoutPaymentCommand(
            Guid orderId, decimal value, string customerEmail,
            string orderCode, Guid customerId,
            List<ProductDTO> products
            )
        {
            AggregateId = orderId;
            OrderId = orderId;
            Value = value;
            CustomerEmail = customerEmail;
            OrderCode = orderCode;
            CustomerId = customerId;
            Products = products;
        }

        public Guid OrderId { get; }
        public decimal Value { get; }
        public string CustomerEmail { get; } = string.Empty;
        public string OrderCode { get; } = string.Empty;
        public Guid CustomerId { get; }
        public List<ProductDTO> Products { get; } = [];

        public override bool IsValid()
        {
            SetValidationResult(new CheckoutPaymentValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
