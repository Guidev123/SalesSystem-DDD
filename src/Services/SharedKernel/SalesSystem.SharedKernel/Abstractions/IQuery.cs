using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions
{
    public interface IQuery<T> : IRequest<Response<T>>
    { }
}