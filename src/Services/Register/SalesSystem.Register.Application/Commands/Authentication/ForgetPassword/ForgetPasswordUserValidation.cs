using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Authentication.ForgetPassword
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
        }
    }
}