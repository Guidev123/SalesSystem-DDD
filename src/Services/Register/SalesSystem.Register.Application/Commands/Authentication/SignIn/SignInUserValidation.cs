using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Authentication.SignIn
{
    public sealed class SignInUserValidation : AbstractValidator<SignInUserCommand>
    {
        public SignInUserValidation()
        {
            
        }
    }
}
