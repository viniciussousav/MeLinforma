using Domain.Enums;

namespace Domain.Shared;

public static class ErrorMessages
{
    public static Error NotificationNotPending(Guid notificationId)
        => new Error("NotificationNotPending", $"Notification {notificationId} is not pending.");
    
    public static Error NotificationNotSent(Guid notificationId)
        => new Error("NotificationNotSent", $"Notification {notificationId} is not in sent status.");
    
    public static Error NotificationAlreadyConfirmed(Guid notificationId)
        => new Error("NotificationAlreadyConfirmed", $"Notification {notificationId} is already confirmed.");
}