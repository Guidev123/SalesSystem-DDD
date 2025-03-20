using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public record GetAllProductsQuery(int PageNumber, int PageSize) : IPagedQuery<GetAllProductsResponse>;
}