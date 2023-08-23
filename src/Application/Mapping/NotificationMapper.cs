using Application.UseCases.CreateNotification;
using Domain.Entities;

namespace Application.Mapping;

public static class NotificationMapper
{
    public static NotificationCreatedEvent MapToNotificationCreatedEvent(this Notification notification)
    {
        return new NotificationCreatedEvent
        {
            Description = notification.Description,
            CustomerId = notification.CustomerId,
            NotificationId = notification.Id,
            Title = notification.Title
        };
    }
}