using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Sales.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
