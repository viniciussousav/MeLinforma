using Application.Events;
using Domain.Entities;
using Domain.Shared;

namespace Application.Mapping;

public static class NotificationMapper
{
    public static NotificationCreated MapToNotificationCreated(this Notification notification)
    {
        var delayInternalMs = notification.SendAt.Millisecond - DateTimeOffset.UtcNow.Millisecond;
        
        return new NotificationCreated
        {
            Description = notification.Description,
            CustomerId = notification.CustomerId,
            NotificationId = notification.Id,
            Title = notification.Title,
            SentAt = notification.SendAt,
            DeliveryDelay = Math.Clamp(delayInternalMs, 0, Math.Abs(delayInternalMs))
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