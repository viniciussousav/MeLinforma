using Application.Events;
using Domain.Entities;
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
            HasDelay = DateTimeOffset.Now < notification.SendAt
        };
    }
    
    public static NotificationFailed MapToNotificationFailed(this Notification notification, IEnumerable<Error> errors)
    {
        return new NotificationFailed
        {
            NotificationId = notification.Id,
            Errors = errors
        };
    }
}