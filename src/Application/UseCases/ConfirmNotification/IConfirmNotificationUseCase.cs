using Domain.Shared;

namespace Application.UseCases.ConfirmNotification;

public interface IConfirmNotificationUseCase
{
    Task Execute(ConfirmNotificationCommand command);
}