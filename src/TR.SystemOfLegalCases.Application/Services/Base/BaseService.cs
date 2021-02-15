using FluentValidation;
using FluentValidation.Results;
using TR.SystemOfLegalCases.Application.Notifications;
using TR.SystemOfLegalCases.Application.Notifications.Interfaces;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;

namespace TR.SystemOfLegalCases.Application.Services.Base
{
    public abstract class BaseService
    {
        protected readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected void Notify(string message, params object[] parameters)
        {
            _notifier.Handle(new Notification(string.Format(message, parameters)));
        }

        protected bool RunValidation<TV, TE>(TV validation, TE entity)
            where TE : Entity
            where TV : AbstractValidator<TE>
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }

        protected bool RunSimpleValidation<TV, TE>(TV validation, TE entity)
            where TE : class
            where TV : AbstractValidator<TE>
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}
