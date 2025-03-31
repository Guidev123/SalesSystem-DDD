using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Customers.AddAddress
{
    public sealed class AddAddressValidation : AbstractValidator<AddAddressCommand>
    {
        public AddAddressValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .NotEmpty()
                .NotNull()
                .WithMessage("Customer Id is required.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required")
                .Length(5, 100).WithMessage("Street name must be between 5 and 100 characters");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Number is required")
                .Matches(@"^\d+$").WithMessage("Number must be a valid number");

            RuleFor(x => x.AdditionalInfo)
                .MaximumLength(200).WithMessage("Additional info must be up to 200 characters");

            RuleFor(x => x.Neighborhood)
                .NotEmpty().WithMessage("Neighborhood is required")
                .Length(3, 100).WithMessage("Neighborhood name must be between 3 and 100 characters");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("Zip code must be in the format 00000-000");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required")
                .Length(2).WithMessage("State must have 2 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .Length(3, 100).WithMessage("City name must be between 3 and 100 characters");
        }
    }
}