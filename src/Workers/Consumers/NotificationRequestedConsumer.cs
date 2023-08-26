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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationRequestedConsumer(
        ILogger<NotificationRequestedConsumer> logger, 
        IBus bus, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _bus = bus;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Consume(ConsumeContext<NotificationRequested> context)
    {
        try
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<ICreateNotificationUseCase>();
                        
            var message = context.Message;
            var command = message.MapToCreateNotificationCommand();
            
            var result = await useCase.Execute(command);
            
            if (!result.IsValid)
            {
                var notificationFailed = result.Value!.MapToNotificationFailed(result.Errors);
                await PublishNotificationFailed(notificationFailed);
            }
            
            var notificationCreated = result.Value!.MapToNotificationCreated();

            if (notificationCreated.IsFutureMessage)
            {
                await PublishScheduledMessage(notificationCreated);
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
    
    private async Task PublishScheduledMessage(NotificationCreated notificationCreated)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var scheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();
        
        await scheduler.SchedulePublish(notificationCreated.SentAt.DateTime, notificationCreated);
        _logger.LogWarning("Notification {NotificationId} produced to schedule time", notificationCreated.NotificationId);
    }
    
    private async Task PublishNotificationFailed(NotificationFailed message)
    {
        await _bus.Publish(message);
        _logger.LogWarning("Notification {NotificationId} failed message produced", message.NotificationId);
    }
}