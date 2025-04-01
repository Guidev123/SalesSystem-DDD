using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.ForgetPassword
{
    public record ForgetPasswordUserCommand : Command<ForgetPasswordUserResponse>
    {
        public ForgetPasswordUserCommand(string email, string clientUrlToResetPassword)
        {
            Email = email;
            ClientUrlToResetPassword = clientUrlToResetPassword;
        }

        public string Email { get; }
        public string ClientUrlToResetPassword { get; private set; }

        public override bool IsValid()
        {
            SetValidationResult(new ForgetPasswordUserValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}