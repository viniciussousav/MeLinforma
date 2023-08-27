using Application.UseCases.CreateNotification;
using Domain.Entities;

namespace Application.Mapping;

public static class CreateNotificationCommandMapper
{
    public static Notification MapToNotification(this CreateNotificationCommand command)
    {
        return new Notification(
            command.NotificationId, command.CustomerId, command.Title, command.Description, command.SendAt, command.Type);
    }
}