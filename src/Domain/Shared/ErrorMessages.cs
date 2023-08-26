using Domain.Enums;

namespace Domain.Shared;

public static class ErrorMessages
{
    public static Error NotificationAlreadyConfirmed(Guid notificationId, NotificationStatus status)
        => new Error("NotificationAlreadyConfirmed", $"Notification {notificationId} can not be set to {status}");
}