using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Register.Application.Commands.Authentication.ResetPassword
{
    public record ResetPasswordUserCommand : Command<ResetPasswordUserResponse>
    {
        public ResetPasswordUserCommand(string password, string confirmPassword, string email, string token)
        {
            Password = password;
            ConfirmPassword = confirmPassword;
            Email = email;
            Token = token;
        }

        public string Password { get; }
        public string ConfirmPassword { get; }
        public string Email { get; }
        public string Token { get; }

        public override bool IsValid()
        {
            SetValidationResult(new ResetPasswordUserValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}