using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Authentication.ResetPassword
{
    public sealed class ResetPasswordUserValidation : AbstractValidator<ResetPasswordUserCommand>
    {
        public ResetPasswordUserValidation()
        {
            
        }
    }
}
