using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Register.Application.Commands.Customers.AddAddress
{
    public record AddAddressCommand : Command<AddAddressResponse>
    {
        public AddAddressCommand(string street, string number, string additionalInfo,
                                 string neighborhood, string zipCode,
                                 string city, string state)
        {
            Street = street;
            Number = number;
            AdditionalInfo = additionalInfo;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public string Street { get; } = string.Empty;
        public string Number { get; } = string.Empty;
        public string AdditionalInfo { get; } = string.Empty;
        public string Neighborhood { get; } = string.Empty;
        public string ZipCode { get; } = string.Empty;
        public string City { get; } = string.Empty;
        public string State { get; } = string.Empty;
        public Guid CustomerId { get; private set; }

        public void SetCustomerId(Guid customerId)
        {
            AggregateId = customerId;
            CustomerId = customerId;
        }
        public override bool IsValid()
        {
            SetValidationResult(new AddAddressValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}