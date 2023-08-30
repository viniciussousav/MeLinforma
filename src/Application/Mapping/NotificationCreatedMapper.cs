using Application.UseCases.SendNotification;
using Domain.Events;
using Domain.Shared;

namespace Application.Mapping;

public static class NotificationCreatedMapper
{
    public static NotificationFailed MapToNotificationFailed(this NotificationCreated message, params Error[] errors)
    {
        return new NotificationFailed
        {
            NotificationId = message.NotificationId,
            Errors = errors
        };
    }
    
    public static SendNotificationCommand MapToSendNotificationCommand(this NotificationCreated message)
    {
        return new SendNotificationCommand(message.NotificationId);
    }
    
    public static NotificationSent MapToNotificationSent(this NotificationCreated message)
    {
        return new NotificationSent
        {
            NotificationId = message.NotificationId,
        };
    }
}