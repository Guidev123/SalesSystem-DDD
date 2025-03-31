using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Domain.Entities;

namespace SalesSystem.Payments.Application.Mappers
{
    public static class PaymentMappers
    {
        public static Payment MapToPayment(this CheckoutPaymentCommand command)
            => new(command.OrderId, command.CustomerId, command.Value);
    }
}