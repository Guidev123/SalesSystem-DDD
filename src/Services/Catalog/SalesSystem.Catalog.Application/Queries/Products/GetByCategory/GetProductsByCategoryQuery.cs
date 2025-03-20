using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public record GetProductsByCategoryQuery(int PageNumber, int PageSize, int Code) : IPagedQuery<GetProductsByCategoryResponse>;
}