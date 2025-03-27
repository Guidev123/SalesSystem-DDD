using SalesSystem.Register.Domain.ValueObjects;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Register.Domain.Entities
{
    public class Customer : Entity, IAggregateRoot
    {
        private const int MIN_AGE = 16;

        public Customer(Guid id, string name, string email, string document, DateTime birthDate)
        {
            Id = id;
            Name = name;
            Email = new(email);
            Document = new(document);
            BirthDate = birthDate;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            Validate();
        }

        private Customer() { }

        public string Name { get; private set; } = string.Empty;
        public Email Email { get; private set; } = null!;
        public Document Document { get; private set; } = null!;
        public DateTime BirthDate { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Address? Address { get; private set; }

        public void ChangeEmail(string email)
        {
             Email = new(email);
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        public override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, "Customer name cannot be empty.");
            AssertionConcern.EnsureLengthInRange(Name, 3, 100, "Customer name must be between 3 and 100 characters.");

            AssertionConcern.EnsureNotNull(Email.Address, "Customer email cannot be null.");

            AssertionConcern.EnsureNotNull(Document.Number, "Customer document cannot be null.");

            AssertionConcern.EnsureTrue(BirthDate <= DateTime.Today.AddYears(-MIN_AGE), "User must be at least 16 years old.");

            Address?.Validate();
        }
    }
}
