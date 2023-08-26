using Domain.Shared;

namespace Application.UseCases.FailNotification;

public interface IFailNotificationUseCase
{
    Task Execute(FailNotificationCommand command);
}