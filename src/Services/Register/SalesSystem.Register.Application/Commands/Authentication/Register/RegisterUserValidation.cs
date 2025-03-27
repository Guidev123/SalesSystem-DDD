using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public sealed class RegisterUserValidation : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidation()
        {
            
        }
    }
}
