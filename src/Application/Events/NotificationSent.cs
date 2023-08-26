namespace Application.Events;

public record NotificationSent
{
    public Guid NotificationId { get; init; }
}