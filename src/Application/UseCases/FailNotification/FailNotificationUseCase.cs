using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.FailNotification;

public class FailNotificationUseCase : IFailNotificationUseCase
{
    private readonly ILogger<FailNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;

    public FailNotificationUseCase(ILogger<FailNotificationUseCase> logger, INotificationRepository notificationRepository)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
    }

    public async Task Execute(FailNotificationCommand command)
    {
        try
        {
            var notification = await _notificationRepository.Get(command.NotificationId);
            
            if (notification == Notification.Empty)
            {
                _logger.LogInformation("Notification {NotificationId} does not exists", command.NotificationId);
                return;
            }
            
            if (notification.Status == NotificationStatus.Failed)
            {
                _logger.LogInformation("Notification {NotificationId} is already failed", command.NotificationId);
                return;
            }
            
            notification.Fail();
            await _notificationRepository.Update(notification);
            
            _logger.LogInformation("Notification {NotificationId} updated to failed", command.NotificationId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An unexpected exception occured handling Notification {NotificationId}", command.NotificationId);
            throw;
        }
    }
}