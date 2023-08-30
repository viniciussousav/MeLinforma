using Domain.Entities;
using Domain.Shared;

namespace Application.UseCases.SendNotification;

public interface ISendNotificationUseCase
{
    Task<Result<Notification>> Execute(SendNotificationCommand command);
}