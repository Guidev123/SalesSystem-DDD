using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public record RegisterUserCommand : Command<RegisterUserResponse>
    {
        public RegisterUserCommand(string name, string document, string email, string password, string confirmPassword, DateTime birthDate)
        {
            Name = name;
            Document = document;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            BirthDate = birthDate;
        }

        public string Name { get; } = string.Empty;
        public string Document { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public DateTime BirthDate { get; }
        public string Password { get; } = string.Empty;
        public string ConfirmPassword { get; } = string.Empty;

        public override bool IsValid()
        {
            SetValidationResult(new RegisterUserValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}