using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.SendNotification;

public class SendNotificationUseCase : ISendNotificationUseCase
{
    private readonly ILogger<SendNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;
    // private readonly IWebNotificationHub _notificationHub;

    public SendNotificationUseCase(
        ILogger<SendNotificationUseCase> logger, 
        // IWebNotificationHub notificationHub, 
        INotificationRepository notificationRepository)
    {
        _logger = logger;
        // _notificationHub = notificationHub;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<EmptyResult>> Execute(SendNotificationCommand command)
    {
        try
        {
            var notification = await _notificationRepository.Get(command.NotificationId);

            if (notification == Notification.Empty)
            {
                _logger.LogError("Notification {NotificationId} does not exist at {SendNotificationUseCase}",
                    command.NotificationId, nameof(SendNotificationUseCase));
                return Result.Fail<EmptyResult>(ErrorMessages.NotificationNotFound(command.NotificationId));
            }

            if (notification.Status == NotificationStatus.Succeeded)
            {
                _logger.LogWarning("Notification {NotificationId} was already sent, skipping to avoid duplicated notifications", command.NotificationId);
                return Result.Skip<EmptyResult>();
            }

            // switch (command.Type)
            // {
            //     case NotificationType.Web:
            //         await _notificationHub.TryNotifyNow(command.CustomerId, command.Title, command.Description);
            //         break;
            //     default:
            //         throw new DomainException(ErrorMessages.NotSupportedNotificationType(command.NotificationId));
            // }
            
            notification.Sent();
            await _notificationRepository.Update(notification);
            
            return Result.Success(EmptyResult.Empty);
        }
        catch (DomainException e)
        {
            _logger.LogWarning(e, "An DomainException occured sending Notification {NotificationId}, skipping to avoid duplicated notifications", command.NotificationId);
            return Result.Skip<EmptyResult>();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An unexpected exception occured handling Notification {NotificationId}", command.NotificationId);
            throw;
        }
    }
}