using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.Business.Models;

namespace SalesSystem.Payments.ACL.Interfaces
{
    public interface IStripeService
    {
        Task<string?> CreateSessionAsync(Order order, StripeConfiguration stripeConfiguration);
    }
}
