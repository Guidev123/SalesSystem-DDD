using Microsoft.AspNetCore.Mvc;
using SalesSystem.SharedKernel.Communication.Mediator;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/sales")]
    public class SalesController(IMediatorHandler mediatorHandler) : MainController
    {
        [HttpGet]
        public async Task<IResult> GetOrderByCustomerAsync()
            => CustomResponse(await mediatorHandler.SendQuery(new Get))
    }
}