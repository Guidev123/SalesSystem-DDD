using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Configuration;
using SalesSystem.Catalog.Application.Products.Commands.Create;
using SalesSystem.Catalog.Application.Products.Commands.Update;
using SalesSystem.Catalog.Application.Products.Queries.GetAll;
using SalesSystem.Catalog.Application.Products.Queries.GetAllCategories;
using SalesSystem.Catalog.Application.Products.Queries.GetByCategory;
using SalesSystem.Catalog.Application.Products.Queries.GetById;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Enums;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/catalog")]
    public class CatalogController(IMediatorHandler mediatorHandler,
                                   IHttpContextAccessor httpContextAccessor)
                                 : MainController(httpContextAccessor)
    {
        [HttpGet]
        public async Task<IResult> GetAllAsync(int pageNumber = ApiConfiguration.DEFAULT_PAGE_NUMBER, int pageSize = ApiConfiguration.DEFAULT_PAGE_SIZE)
            => CustomResponse(await mediatorHandler.SendQueryAsync(new GetAllProductsQuery(pageNumber, pageSize)).ConfigureAwait(false));

        [HttpGet("{id:guid}")]
        public async Task<IResult> GetByIdAsync(Guid id)
            => CustomResponse(await mediatorHandler.SendQueryAsync(new GetProductByIdQuery(id)).ConfigureAwait(false));

        [HttpGet("{code}")]
        public async Task<IResult> GetAllByCategoryAsync(int code, int pageNumber = ApiConfiguration.DEFAULT_PAGE_NUMBER, int pageSize = ApiConfiguration.DEFAULT_PAGE_SIZE)
            => CustomResponse(await mediatorHandler.SendQueryAsync(new GetProductsByCategoryQuery(pageNumber, pageSize, code)).ConfigureAwait(false));

        [HttpGet("category")]
        public async Task<IResult> GetAllCategoriesAsync(int pageNumber = ApiConfiguration.DEFAULT_PAGE_NUMBER, int pageSize = ApiConfiguration.DEFAULT_PAGE_SIZE)
            => CustomResponse(await mediatorHandler.SendQueryAsync(new GetAllCategoriesQuery(pageNumber, pageSize)).ConfigureAwait(false));

        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpPost]
        public async Task<IResult> CreateAsync(CreateProductCommand command)
            => CustomResponse(await mediatorHandler.SendCommandAsync(command).ConfigureAwait(false));

        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpPut]
        public async Task<IResult> UpdateAsync(UpdateProductCommand command)
            => CustomResponse(await mediatorHandler.SendCommandAsync(command).ConfigureAwait(false));
    }
}