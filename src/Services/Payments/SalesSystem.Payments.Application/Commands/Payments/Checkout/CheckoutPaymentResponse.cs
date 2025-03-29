namespace SalesSystem.Payments.Application.Commands.Payments.Checkout
{
    public record CheckoutPaymentResponse(string SessionId, string OrderCode);
}