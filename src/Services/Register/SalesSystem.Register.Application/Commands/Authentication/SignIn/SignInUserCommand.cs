using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Register.Application.Commands.Authentication.SignIn
{
    public record SignInUserCommand : Command<SignInUserResponse>
    {
        public SignInUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }

        public override bool IsValid()
        {
            SetValidationResult(new SignInUserValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}