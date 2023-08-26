namespace Application.UseCases.ConfirmNotification;

public record ConfirmNotificationCommand {
    public Guid NotificationId { get; init; }
};