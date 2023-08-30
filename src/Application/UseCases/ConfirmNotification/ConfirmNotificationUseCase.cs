using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ConfirmNotification;

public class ConfirmNotificationUseCase : IConfirmNotificationUseCase
{
    private readonly ILogger<ConfirmNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;

    public ConfirmNotificationUseCase(ILogger<ConfirmNotificationUseCase> logger, INotificationRepository notificationRepository)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
    }
    
    public async Task Execute(ConfirmNotificationCommand command)
    {
        try
        {
            var notification = await _notificationRepository.Get(command.NotificationId);

            if (notification == Notification.Empty)
            {
                _logger.LogInformation("Notification {NotificationId} does not exists", command.NotificationId);
                return;
            }
            
            if (notification.Status == NotificationStatus.Succeeded)
            {
                _logger.LogInformation("Notification {NotificationId} is already confirmed", command.NotificationId);
                return;
            }
            
            notification.Confirm();
            await _notificationRepository.Update(notification);
            
            _logger.LogInformation("Notification {NotificationId} successfully confirmed", command.NotificationId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An unexpected exception occured handling Notification {NotificationId}", command.NotificationId);
            throw;
        }
    }
}