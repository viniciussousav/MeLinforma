namespace Application.UseCases.SendNotification;

public record SendNotificationCommand
{
    public Guid NotificationId { get; init; }
    public Guid CustomerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}