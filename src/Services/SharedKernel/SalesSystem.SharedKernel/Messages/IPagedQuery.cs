using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Messages
{
    public interface IPagedQuery<T> : IRequest<PagedResponse<T>>
    { }
}