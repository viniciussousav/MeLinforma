using Domain.Enums;

namespace Application.UseCases.SendNotification;

public record SendNotificationCommand(Guid NotificationId);