using Domain.Enums;

namespace Domain.Entities;

public class Notification
{
    public static Notification Empty = new();

    protected Notification() { }
     
    public Notification(Guid customerId, string title, string description, DateTimeOffset sendAt, NotificationType type)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Title = title;
        Description = description;
        SendAt = sendAt;
        Type = type;
    }

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public string Title { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    public DateTimeOffset SendAt { get; }
    public NotificationType Type { get; }
    public NotificationStatus Status { get; private set; }
}