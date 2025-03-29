using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Commands.Authentication.ResetPassword
{
    public record ResetPasswordUserCommand : Command<ResetPasswordUserResponse>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}