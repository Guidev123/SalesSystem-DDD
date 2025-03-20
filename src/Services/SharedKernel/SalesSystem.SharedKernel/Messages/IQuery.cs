using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Messages
{
    public interface IQuery<T> : IRequest<Response<T>>
    { }
}