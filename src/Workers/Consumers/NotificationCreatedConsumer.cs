using Application.Mapping;
using Application.UseCases.SendNotification;
using Domain.Events;
using MassTransit;

namespace Workers.Consumers;

public class NotificationCreatedConsumer : IConsumer<NotificationCreated>
{
    private readonly ILogger<NotificationCreatedConsumer> _logger;
    private readonly IBus _bus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationCreatedConsumer(ILogger<NotificationCreatedConsumer> logger, IBus bus, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _bus = bus;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Consume(ConsumeContext<NotificationCreated> context)
    {
        try
        {
            var notificationCreated = context.Message;
            var command = notificationCreated.MapToSendNotificationCommand();
            
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<ISendNotificationUseCase>();

            var result = await useCase.Execute(command);

            if (!result.IsValid)
            {
                var notificationFailed = notificationCreated.MapToNotificationFailed();
                await _bus.Publish(notificationFailed);
                _logger.LogWarning("Notification {NotificationId} failed message produced", notificationCreated.NotificationId);
                return;
            }

            var notificationSent = notificationCreated.MapToNotificationSent();
            await _bus.Publish(notificationSent);
            
            _logger.LogWarning("Notification {NotificationId} confirmation message produced", notificationCreated.NotificationId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occured at {NotificationCreatedConsumer}", nameof(NotificationCreatedConsumer));
            throw;
        }
    }
}