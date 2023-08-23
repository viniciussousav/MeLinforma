namespace Application.UseCases.CreateNotification;

public record NotificationCreatedEvent
{
    public Guid NotificationId { get; set; }
    public Guid CustomerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}