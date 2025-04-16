using FluentValidation;
using SalesSystem.Registers.Domain.ValueObjects;

namespace SalesSystem.Registers.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerValidation : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer id invalid.");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name can not be empty.");

            RuleFor(c => c.Document)
                .Must(DocumentIsValid)
                .WithMessage("Invalid Document.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("Invalid E-mail.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("The {PropertyName} field cannot be empty.")
                .Must(IsValidAge).WithMessage("You need to be over 16 years old.");
        }

        private static bool IsValidAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age)) age--;

            return age >= 16;
        }

        private static bool DocumentIsValid(string cpf) => Document.IsValid(cpf);
    }
}