using FluentValidation;
using System.Text.RegularExpressions;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserValidation : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The {PropertyName} field cannot be empty.")
                .Length(2, 100).WithMessage("The {PropertyName} must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The {PropertyName} field cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field cannot be empty.")
                .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
                .Must(HasUpperCase).WithMessage("The password must contain at least one uppercase letter.")
                .Must(HasLowerCase).WithMessage("The password must contain at least one lowercase letter.")
                .Must(HasDigit).WithMessage("The password must contain at least one digit.")
                .Must(HasSpecialCharacter).WithMessage("The password must contain at least one special character (!@#$%^&* etc.).");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The passwords do not match")
                    .When(x => !string.IsNullOrEmpty(x.Password))
                    .NotEmpty().WithMessage("The password field cannot be empty.")
                    .MinimumLength(8).WithMessage("The password must be at least 8 characters long.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("The {PropertyName} field cannot be empty.")
                .Must(IsValidAge).WithMessage("You need to be over 16 years old.");
        }

        private static bool HasUpperCase(string password) => password.Any(char.IsUpper);

        private static bool HasLowerCase(string password) => password.Any(char.IsLower);

        private static bool HasDigit(string password) => password.Any(char.IsDigit);

        private static bool IsValidAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age)) age--;

            return age >= 16;
        }

        private static bool HasSpecialCharacter(string password)
        {
            var specialCharRegex = new Regex(@"[!@#$%^&*(),.?""{}|<>]");
            return specialCharRegex.IsMatch(password);
        }
    }
}