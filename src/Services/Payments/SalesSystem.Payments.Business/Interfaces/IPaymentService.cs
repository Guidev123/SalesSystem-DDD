using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Communication.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Business.Interfaces
{
    public interface IPaymentService 
    {
        Task<Response<Transaction>> CheckoutOrderAsync(PaymentOrderDTO paymentOrder);
    }
}
