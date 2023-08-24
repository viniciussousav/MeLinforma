using Domain.Enums;

namespace Application.Events;

public record NotificationRequested
{
    public NotificationDetails NotificationInfo { get; init; } = NotificationDetails.Empty;

    public record NotificationDetails
    {
        public static readonly NotificationDetails Empty = new();
        
        public Guid NotificationId { get; init; }
        public Guid CustomerId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset SendAt { get; init; }
        public NotificationType Type { get; init; }
    }
}