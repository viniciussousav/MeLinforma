using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.SendNotification;

public class SendNotificationUseCase : ISendNotificationUseCase
{
    private readonly ILogger<SendNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationsHub> _notificationsHub;

    public SendNotificationUseCase(
        ILogger<SendNotificationUseCase> logger, 
        IHubContext<NotificationsHub> notificationsHub, 
        INotificationRepository notificationRepository)
    {
        _logger = logger;
        _notificationsHub = notificationsHub;
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

            await SendNotification(notification);
            
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

    private async Task SendNotification(Notification notification)
    {
        try
        {
            switch (notification.Type)
            {
                case NotificationType.Web:
                    await _notificationsHub.Clients.All.SendAsync(
                        notification.CustomerId.ToString(), $"{notification.Title} - {notification.Description}");
                    break;
                default:
                    throw new DomainException(ErrorMessages.NotSupportedNotificationType(notification.Id));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An unexpected exception occured on {SendNotification} for notification {NotificationId}",
                nameof(SendNotification), notification.Id);
        }
    }
}