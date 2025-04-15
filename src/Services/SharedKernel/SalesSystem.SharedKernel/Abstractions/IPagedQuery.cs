using MidR.Interfaces;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions
{
    public interface IPagedQuery<T> : IRequest<PagedResponse<T>>
    { }
}