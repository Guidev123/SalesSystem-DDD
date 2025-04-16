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

        protected Category()
        { }

        public string Name { get; } = string.Empty;
        public int Code { get; }

        public override string ToString() => $"{Name}-{Code}";

        public override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, "The field 'Name' cannot be empty. Please provide a valid product name.");
            AssertionConcern.EnsureDifferent(Code, 0, "The field 'Code' cannot be 0");
        }
    }
}