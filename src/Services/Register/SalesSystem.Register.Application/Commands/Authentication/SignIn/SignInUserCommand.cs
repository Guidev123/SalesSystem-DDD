using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.SignIn
{
    public record SignInUserCommand : Command<SignInUserResponse>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
