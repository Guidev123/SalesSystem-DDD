using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public record RegisterUserCommand : Command<RegisterUserResponse>
    {
        public RegisterUserCommand(string name, string cpf, string email, string password, string confirmPassword)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public string Name { get; } = string.Empty;
        public string Cpf { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public string ConfirmPassword { get; } = string.Empty;

        public override bool IsValid()
        {
            SetValidationResult(new RegisterUserValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
