using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.Business.Models;

namespace SalesSystem.Payments.ACL.Services
{
    public sealed class StripeService : IStripeService
    {
        public Task<string?> CreateSessionAsync(Order order, StripeConfiguration stripeConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}
