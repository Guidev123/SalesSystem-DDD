using FluentValidation.Results;
using MidR.Interfaces;
using SalesSystem.SharedKernel.Messages;
using SalesSystem.SharedKernel.Responses;
using System.Text.Json.Serialization;

namespace SalesSystem.SharedKernel.Abstractions
{
    public abstract record Command<T> : Message, IRequest<Response<T>>
    {
        protected Command() => Timestamp = DateTime.Now;
        public DateTime Timestamp { get; private set; }
        [JsonIgnore]
        public ValidationResult? ValidationResult { get; private set; }

        protected void SetValidationResult(ValidationResult validationResult)
            => ValidationResult = validationResult;

        public List<string> GetErrorMessages()
            => ValidationResult!.Errors.Select(x => x.ErrorMessage).ToList();

        public void AddError(string errorMessage)
            => ValidationResult!.Errors.Add(new ValidationFailure { ErrorMessage = errorMessage });

        public abstract bool IsValid();
    }
}