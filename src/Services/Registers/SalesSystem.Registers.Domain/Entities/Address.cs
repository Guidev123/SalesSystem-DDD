using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Registers.Domain.Entities
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

        private Address()
        { }

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
            AssertionConcern.EnsureLengthInRange(Street, 5, 100, "Street must be between 5 and 100 characters.");

            AssertionConcern.EnsureNotEmpty(Number, "Number cannot be empty.");
            AssertionConcern.EnsureMatchesPattern(@"^\d+$", Number, "Number must be a valid number.");

            AssertionConcern.EnsureMaxLength(AdditionalInfo, 200, "Additional info must be up to 200 characters.");

            AssertionConcern.EnsureNotEmpty(Neighborhood, "Neighborhood cannot be empty.");
            AssertionConcern.EnsureLengthInRange(Neighborhood, 3, 100, "Neighborhood must be between 3 and 100 characters.");

            AssertionConcern.EnsureNotEmpty(ZipCode, "Zip code cannot be empty.");
            AssertionConcern.EnsureMatchesPattern(@"^\d{5}-\d{3}$", ZipCode, "Zip code must be in the format 00000-000.");

            AssertionConcern.EnsureNotEmpty(State, "State cannot be empty.");
            AssertionConcern.EnsureLengthInRange(State, 2, 2, "State must have exactly 2 characters.");

            AssertionConcern.EnsureNotEmpty(City, "City cannot be empty.");
            AssertionConcern.EnsureLengthInRange(City, 3, 100, "City must be between 3 and 100 characters.");
        }
    }
}