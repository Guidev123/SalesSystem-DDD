using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.ForgetPassword
{
    public record ForgetPasswordUserCommand : Command<ForgetPasswordUserResponse>
    {
        public ForgetPasswordUserCommand(string email) => Email = email;
        
        public string Email { get; }

        public override bool IsValid()
        {
            SetValidationResult(new ForgetPasswordUserValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}
