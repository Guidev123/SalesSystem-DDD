using FluentValidation;

namespace SalesSystem.Register.Application.Commands.Authentication.Delete
{
    public sealed class DeleteUserValidation : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("User id invalid");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("Invalid E-mail");
        }
    }
}