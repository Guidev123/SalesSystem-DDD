using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.SharedKernel.Data
{
    public interface IRepository<T> : IDisposable where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
