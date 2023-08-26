using Application.Events;
using Application.UseCases.CreateNotification;

namespace Application.Mapping;

public static class NotificationRequestedMapper
{
    public static CreateNotificationCommand MapToCreateNotificationCommand(this Events.NotificationRequested request)
    {
        return new CreateNotificationCommand(
            request.NotificationInfo.NotificationId,
            request.NotificationInfo.CustomerId,
            request.NotificationInfo.Title,
            request.NotificationInfo.Description,
            request.NotificationInfo.SendAt,
            request.NotificationInfo.Type);
    }
}