using FluentValidation;

namespace SalesSystem.Registers.Application.Commands.Authentication.Delete
{
    public sealed class DeleteUserValidation : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("User id invalid");
        }
    }
}