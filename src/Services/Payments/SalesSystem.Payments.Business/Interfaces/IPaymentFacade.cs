using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Business.Interfaces
{
    public interface IPaymentFacade
    {
        Task<Response<Transaction>> MakePaymentAsync(Order order);
    }
}
