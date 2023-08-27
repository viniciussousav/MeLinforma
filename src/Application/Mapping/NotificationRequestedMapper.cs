using Application.UseCases.CreateNotification;
using Domain.Events;
using Domain.Shared;

namespace Application.Mapping;

public static class NotificationRequestedMapper
{
    public static CreateNotificationCommand MapToCreateNotificationCommand(this NotificationRequested request)
    {
        return new CreateNotificationCommand(
            request.NotificationId,
            request.CustomerId,
            request.Title,
            request.Description,
            request.SendAt,
            request.Type);
    }
    
    public static NotificationFailed MapToNotificationFailed(this NotificationRequested request, IEnumerable<Error> errors)
    {
        return new NotificationFailed
        {
            NotificationId = request.NotificationId,
            Errors = errors
        };
    }
}