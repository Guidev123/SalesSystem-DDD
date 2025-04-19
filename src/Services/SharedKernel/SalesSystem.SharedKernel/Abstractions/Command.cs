using MidR.Interfaces;
using SalesSystem.SharedKernel.Messages;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions
{
    public abstract record Command<T> : Message, IRequest<Response<T>>
    {
        protected Command() => Timestamp = DateTime.Now;
        public DateTime Timestamp { get; private set; }
    }
}