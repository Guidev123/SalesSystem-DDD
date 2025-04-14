using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Register.Application.Commands.Customers.Create
{
    public record CreateCustomerCommand : Command<CreateCustomerResponse>
    {
        public CreateCustomerCommand(Guid id, string name, string email, string document, DateTime birthDate)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Document = document;
            BirthDate = birthDate;
        }
        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public string Document { get; } = string.Empty;
        public DateTime BirthDate { get; }
        public override bool IsValid()
        {
            SetValidationResult(new CreateCustomerValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}