namespace Application.Events;

public record NotificationCreated
{
    public Guid NotificationId { get; init; }
    public Guid CustomerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset SentAt { get; init; }
    public long DeliveryDelay { get; init; }

    public bool HasDelay => DeliveryDelay > 0;
}