using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Products.Queries.GetByCategory
{
    public record GetProductsByCategoryQuery(int PageNumber, int PageSize, int Code) : IPagedQuery<GetProductsByCategoryResponse>;
}