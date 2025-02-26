using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.Entities
{
    public class Category : Entity
    {
        public Category(string name, int code)
        {
            Name = name;
            Code = code;
            Validate();
        }

        public string Name { get; }
        public int Code { get; }

        public override string ToString() => $"{Name}-{Code}";

        public override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, "The field 'Name' cannot be empty. Please provide a valid product name.");
            AssertionConcern.EnsureEqual(Code, 0, "The field 'Code' cannot be 0");
        }
    }
}
