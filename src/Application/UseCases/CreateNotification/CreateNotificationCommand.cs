using Domain.Enums;

namespace Application.UseCases.CreateNotification;

public record CreateNotificationCommand
{
    public Guid NotificationId { get; init; }
    public Guid CustomerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset SendAt { get; init; }
    public NotificationType Type { get; init; }
    
}