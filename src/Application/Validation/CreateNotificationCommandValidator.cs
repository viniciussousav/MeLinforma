using Application.UseCases.CreateNotification;
using Domain.Enums;
using FluentValidation;

namespace Application.Validation;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.NotificationId).NotEqual(Guid.Empty);
        RuleFor(x => x.CustomerId).NotEqual(Guid.Empty);
        RuleFor(x => x.Title).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(4);
        RuleFor(x => x.SendAt).NotEqual(DateTimeOffset.MinValue);
        RuleFor(x => x.Type).NotEqual(NotificationType.Undefined);
    }
}