using Domain.Shared;

namespace Application.UseCases.SendNotification;

public interface ISendNotificationUseCase
{
    Task<Result<EmptyResult>> Execute(SendNotificationCommand command);
}