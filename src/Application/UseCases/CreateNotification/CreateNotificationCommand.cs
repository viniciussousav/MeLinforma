using Domain.Enums;

namespace Application.UseCases.CreateNotification;

public record CreateNotificationCommand(
    Guid NotificationId, 
    Guid CustomerId, 
    string Title, 
    string Description, 
    DateTimeOffset SendAt, 
    NotificationType Type);