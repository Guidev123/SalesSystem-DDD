using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Products.Queries.GetAll
{
    public record GetAllProductsQuery(int PageNumber, int PageSize) : IPagedQuery<GetAllProductsResponse>;
}