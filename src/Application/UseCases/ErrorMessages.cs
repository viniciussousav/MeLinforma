using Domain.Shared;

namespace Application.UseCases;

public static class ErrorMessages
{
    public static Error NotificationNotFound(Guid notificationId)
        => new Error("NotFound", $"Notification {notificationId} not found");
    
    public static Error NotSupportedNotificationType(Guid notificationId)
        => new Error("NotSupported", $"Notification {notificationId} informed type is not supported");
}