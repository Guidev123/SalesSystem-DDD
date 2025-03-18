using MediatR;
using SalesSystem.SharedKernel.Messages;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesQuery() : IPagedQuery<GetAllCategoriesResponse>;
}
