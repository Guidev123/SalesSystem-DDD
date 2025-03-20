using FluentValidation.Results;
using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Messages
{
    public abstract record Command<T> : Message, IRequest<Response<T>>
    {
        protected Command() => Timestamp = DateTime.Now;
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; private set; } = null!;

        protected void SetValidationResult(ValidationResult validationResult)
            => ValidationResult = validationResult;

        public List<string> GetErrorMessages()
            => ValidationResult.Errors.Select(x => x.ErrorMessage).ToList();

        public void AddError(string errorMessage)
            => ValidationResult.Errors.Add(new ValidationFailure { ErrorMessage = errorMessage });

        public abstract bool IsValid();
    }
}