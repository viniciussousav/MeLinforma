using Domain.Shared;

namespace Application.UseCases.FailNotification;

public record FailNotificationCommand
{
    public Guid NotificationId { get; init; }
    public IEnumerable<Error> Errors { get; init; } = new List<Error>();
}