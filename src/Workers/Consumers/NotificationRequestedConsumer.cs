using Application.Events;
using Application.Mapping;
using Application.UseCases.CreateNotification;
using MassTransit;

namespace Workers.Consumers;

// ReSharper disable once ClassNeverInstantiated.Global
public class NotificationRequestedConsumer : IConsumer<NotificationRequested>
{
    private readonly ILogger<NotificationRequestedConsumer> _logger;
    private readonly IBus _bus;
    private readonly ICreateNotificationUseCase _createNotificationUseCase;

    public NotificationRequestedConsumer(
        ILogger<NotificationRequestedConsumer> logger, 
        IBus bus, 
        ICreateNotificationUseCase createNotificationUseCase)
    {
        _logger = logger;
        _bus = bus;
        _createNotificationUseCase = createNotificationUseCase;
    }

    public async Task Consume(ConsumeContext<NotificationRequested> context)
    {
        try
        {
            var message = context.Message;
            var command = message.MapToCreateNotificationCommand();
            
            var result = await _createNotificationUseCase.Execute(command);

            if (!result.IsValid)
            {
                var notificationFailed = result.Value!.MapToNotificationFailed(result.Errors);
                await _bus.Publish(message: notificationFailed);
                _logger.LogWarning("Notification {NotificationId} failed message produced", command.NotificationId);
            }
            
            var notificationCreated = result.Value!.MapToNotificationCreated();

            if (notificationCreated.HasDelay)
            {
                await _bus.Publish(message: notificationCreated, 
                    callback: publishContext => publishContext.Headers.Set("x-delayed-message", notificationCreated.DeliveryDelay));
                _logger.LogWarning("Notification {NotificationId} produced to schedule time", command.NotificationId);
            }
            
            await _bus.Publish(message: notificationCreated);
            _logger.LogWarning("Notification {NotificationId} produced to be immediately send", command.NotificationId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occured at {NotificationRequestedConsumer}", nameof(NotificationRequestedConsumer));
            throw;
        }
    }
}