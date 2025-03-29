using FluentValidation;
using SalesSystem.Register.Domain.ValueObjects;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public sealed class CreateCustomerValidation : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer id invalid");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name can not be empty");

            RuleFor(c => c.Document)
                .Must(GetCpfValidation)
                .WithMessage("Invalid Document");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("Invalid E-mail");
        }

        protected static bool GetCpfValidation(string cpf) => Document.CpfIsValid(cpf);
    }
}
