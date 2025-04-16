using FluentValidation;

namespace SalesSystem.Registers.Application.Commands.Authentication.ForgetPassword
{
    public sealed class ForgetPasswordUserValidation : AbstractValidator<ForgetPasswordUserCommand>
    {
        public ForgetPasswordUserValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.ClientUrlToResetPassword)
                .NotEmpty()
                .WithMessage("Client Url to reset password is required.")
                .Must(x => x.Contains("http://") || x.Contains("https://"))
                .WithMessage("ClientUrlToResetPassword must be a valid URL.");
        }
    }
}