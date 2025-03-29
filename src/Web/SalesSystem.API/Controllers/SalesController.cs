using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Configuration;
using SalesSystem.Catalog.Application.Queries.Products.GetById;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Application.Queries.Customers.GetAddress;
using SalesSystem.Register.Infrastructure.Models;
using SalesSystem.Sales.Application.Commands.Orders.AddOrderItem;
using SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher;
using SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem;
using SalesSystem.Sales.Application.Commands.Orders.Start;
using SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem;
using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Application.Queries.Orders.GetCustomerCart;
using SalesSystem.Sales.Application.Queries.Orders.GetCustomerOrders;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.API.Controllers
{
    [Authorize]
    [Route("api/v1/sales")]
    public class SalesController(IMediatorHandler mediatorHandler,
                                 IHttpContextAccessor httpContextAccessor)
                               : MainController(httpContextAccessor)
    {
        [HttpGet("order")]
        public async Task<IResult> GetCustomerOrdersAsync(int pageNumber = ApiConfiguration.DEFAULT_PAGE_NUMBER, int pageSize = ApiConfiguration.DEFAULT_PAGE_SIZE)
            => CustomResponse(await mediatorHandler.SendQuery(new GetCustomerOrdersQuery(pageNumber, pageSize, GetUserId())));

        [HttpGet("cart")]
        public async Task<IResult> GetPurchaseSummaryAsync()
            => CustomResponse(await mediatorHandler.SendQuery(new GetCustomerCartQuery(GetUserId())));

        [HttpPost("cart/item")]
        public async Task<IResult> AddItemToCartAsync(AddOrderItemCommand command)
        {
            var productResponse = await mediatorHandler.SendQuery(new GetProductByIdQuery(command.ProductId));
            if (!productResponse.IsSuccess
                || productResponse.Data is null) CustomResponse(productResponse);

            command.SetCustomerId(GetUserId());
            return CustomResponse(await mediatorHandler.SendCommand(command));
        }

        [HttpPost("cart/apply-voucher")]
        public async Task<IResult> ApplyVoucherToCartAsync(ApplyVoucherCommand command)
        {
            command.SetCustomerId(GetUserId());
            return CustomResponse(await mediatorHandler.SendCommand(command));
        }

        [HttpDelete("cart/item")]
        public async Task<IResult> RemoveOrderItemAsync(RemoveOrderItemCommand command)
        {
            command.SetCustomerId(GetUserId());
            return CustomResponse(await mediatorHandler.SendCommand(command));
        }

        [HttpPut("cart/item")]
        public async Task<IResult> UpdateOrderItemAsync(UpdateOrderItemCommand command)
        {
            command.SetCustomerId(GetUserId());
            return CustomResponse(await mediatorHandler.SendCommand(command));
        }

        [HttpPost("order")]
        public async Task<IResult> StartOrderAsync(CartDTO cart)
        {
            var userId = GetUserId();

            if (!await CustomerHasAddress(userId))
                return CustomerWithoutAddressError();

            var cartResponse = await mediatorHandler.SendQuery(new GetCustomerCartQuery(userId));
            if (!cartResponse.IsSuccess
                || cartResponse.Data is null) CustomResponse(cartResponse);

            return CustomResponse(await mediatorHandler.SendCommand(new StartOrderCommand(cart.OrderId, userId, cart.TotalPrice)));
        }

        private async Task<bool> CustomerHasAddress(Guid customerId)
        {
            var result = await mediatorHandler.SendQuery(new GetAddressByCustomerQuery(customerId)).ConfigureAwait(false);
            return result.IsSuccess && result.Data is not null;
        }

        private static IResult CustomerWithoutAddressError()
        {
            var response = Response<StartOrderResponse>.Failure(["Customer must have a registered address to proceed."]);
            return TypedResults.BadRequest(response);
        }
    }
}