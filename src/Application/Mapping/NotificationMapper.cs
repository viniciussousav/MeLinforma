using Domain.Entities;
using Domain.Events;
using Domain.Shared;

namespace Application.Mapping;

public static class NotificationMapper
{
    public static NotificationCreated MapToNotificationCreated(this Notification notification)
    {
        return new NotificationCreated
        {
            Description = notification.Description,
            CustomerId = notification.CustomerId,
            NotificationId = notification.Id,
            Title = notification.Title,
            SentAt = notification.SendAt,
            IsFutureMessage = DateTimeOffset.Now < notification.SendAt
        };
    }
}