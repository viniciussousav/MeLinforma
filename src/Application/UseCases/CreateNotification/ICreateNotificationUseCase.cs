using Domain.Shared;

namespace Application.UseCases.CreateNotification;

public interface ICreateNotificationUseCase
{
    Task<Result<NotificationCreatedEvent>> Execute(CreateNotificationCommand command);
}