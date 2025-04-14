using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public record GetAllProductsQuery(int PageNumber, int PageSize) : IPagedQuery<GetAllProductsResponse>;
}