using Microsoft.Extensions.Configuration;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.Business.Interfaces;
using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.ACL.Services
{
    public sealed class PaymentFacade(IConfiguration configuration, IStripeService stripeService) : IPaymentFacade
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IStripeService _stripeService = stripeService;
        public async Task<Response<Transaction>> MakePaymentAsync(Order order)
        {
            var apiKey = _configuration["StripeSettings:ApiKey"] ?? string.Empty;
            var frontendUrl = _configuration["StripeSettings:FrontendUrl"] ?? string.Empty;
            var stripeMode = _configuration["StripeSettings:StripeMode"] ?? string.Empty;
            var paymentMethodTypes = _configuration["StripeSettings:PaymentMethodTypes"] ?? string.Empty;

            var stripeResponse = await _stripeService.CreateSessionAsync(order, new(apiKey, frontendUrl, stripeMode, paymentMethodTypes)).ConfigureAwait(false);

            return Response<Transaction>.Success(default);
        }
    }
}
