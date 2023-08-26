using Application.Events;
using Application.UseCases.SendNotification;
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
        return new SendNotificationCommand
        {
            NotificationId = message.NotificationId,
            Description = message.Description,
            Title = message.Title,
            CustomerId = message.CustomerId
        };
    }
    
    public static NotificationSent MapToNotificationSent(this NotificationCreated message)
    {
        return new NotificationSent
        {
            NotificationId = message.NotificationId,
        };
    }
}