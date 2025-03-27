using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Register.Domain.Entities
{
    public class Address : Entity
    {
        public Address(Guid customerId, string street, string number,
                       string additionalInfo, string neighborhood,
                       string zipCode, string city, string state)
        {
            CustomerId = customerId;
            Street = street;
            Number = number;
            AdditionalInfo = additionalInfo;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }
            
        private Address() { }   

        public string Street { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public string AdditionalInfo { get; private set; } = string.Empty;
        public string Neighborhood { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public Customer? Customer { get; private set; }

        public override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Street, "Street cannot be empty.");
            AssertionConcern.EnsureLengthInRange(Street, 3, 100, "Street must be between 3 and 100 characters.");

            AssertionConcern.EnsureNotEmpty(Number, "Number cannot be empty.");
            AssertionConcern.EnsureMaxLength(Number, 10, "Number cannot exceed 10 characters.");

            AssertionConcern.EnsureLengthInRange(AdditionalInfo, 0, 100, "Additional info cannot exceed 100 characters.");

            AssertionConcern.EnsureNotEmpty(Neighborhood, "Neighborhood cannot be empty.");
            AssertionConcern.EnsureLengthInRange(Neighborhood, 3, 100, "Neighborhood must be between 3 and 100 characters.");

            AssertionConcern.EnsureMatchesPattern(@"^\d{8}$", ZipCode, "Zip code must contain exactly 8 digits.");

            AssertionConcern.EnsureNotEmpty(City, "City cannot be empty.");
            AssertionConcern.EnsureLengthInRange(City, 2, 100, "City must be between 2 and 100 characters.");

            AssertionConcern.EnsureNotEmpty(State, "State cannot be empty.");
            AssertionConcern.EnsureLengthInRange(State, 2, 2, "State must have exactly 2 characters.");

            AssertionConcern.EnsureDifferent(Guid.Empty, CustomerId, "Customer Id cannot be empty.s");
        }
    }
}
