using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.Register
{
    public record RegisterUserCommand : Command<RegisterUserResponse>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
