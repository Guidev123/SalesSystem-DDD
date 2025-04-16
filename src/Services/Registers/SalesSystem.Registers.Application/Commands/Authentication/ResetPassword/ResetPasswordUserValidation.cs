using FluentValidation;

namespace SalesSystem.Registers.Application.Commands.Authentication.ResetPassword
{
    public sealed class ResetPasswordUserValidation : AbstractValidator<ResetPasswordUserCommand>
    {
        public ResetPasswordUserValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The NewPassword field is required")
                .Length(6, 100).WithMessage("The NewPassword field must be between {MinLength} and {MaxLength} characters");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("The new passwords do not match")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
        }
    }
}