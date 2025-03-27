using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Register.Domain.ValueObjects
{
    public record Email : ValueObject
    {
        public string Address { get; private set; }

        public Email(string address)
        {
            Address = address;
            Validate();
        }
      
        public override void Validate()
        {
            AssertionConcern.EnsureMatchesPattern(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", Address, "Invalid email format.");
        }
    }
}
