using Domain.Entities;
using Domain.Shared;

namespace Application.UseCases.CreateNotification;

public interface ICreateNotificationUseCase
{
    Task<Result<Notification>> Execute(CreateNotificationCommand command);
}