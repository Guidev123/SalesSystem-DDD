using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Registers.Application.Commands.Authentication.Delete
{
    public record DeleteUserCommand : Command<DeleteUserResponse>
    {
        public DeleteUserCommand(Guid id)
        {
            AggregateId = id;
            Id = id;
        }

        public Guid Id { get; }
        public string Email { get; } = string.Empty;
    }
}