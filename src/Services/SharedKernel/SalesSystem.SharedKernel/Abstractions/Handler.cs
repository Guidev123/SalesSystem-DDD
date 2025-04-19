using FluentValidation;
using FluentValidation.Results;
using SalesSystem.SharedKernel.Notifications;

namespace SalesSystem.SharedKernel.Abstractions
{
    public abstract class Handler(INotificator notificator)
    {
        private readonly INotificator _notificator = notificator;

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var item in validationResult.Errors) Notify(item.ErrorMessage);
        }

        protected List<string> GetNotifications() => _notificator.GetNotifications()?.Select(x => x.Message).ToList() ?? [];

        protected void Notify(string message) => _notificator.HandleNotification(new(message));

        protected bool OperationIsValid() => !_notificator.HasNotifications();

        protected bool ExecuteValidation<TV, TC>(TV validation, TC command)
                       where TV : AbstractValidator<TC>
                       where TC : class
        {
            var validator = validation.Validate(command);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}