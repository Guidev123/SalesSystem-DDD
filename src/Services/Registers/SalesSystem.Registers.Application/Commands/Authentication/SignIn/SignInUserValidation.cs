using FluentValidation;
using System.Text.RegularExpressions;

namespace SalesSystem.Registers.Application.Commands.Authentication.SignIn
{
    public sealed class SignInUserValidation : AbstractValidator<SignInUserCommand>
    {
        public SignInUserValidation()
        {
            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("The {PropertyName} field cannot be empty.")
              .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field cannot be empty.");
        }
    }
}
