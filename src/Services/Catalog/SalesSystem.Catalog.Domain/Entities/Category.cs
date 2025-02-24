using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.Entities
{
    public class Category : Entity
    {
        public Category(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; }
        public int Code { get; }

        public override string ToString() => $"{Name}-{Code}";
    }
}
