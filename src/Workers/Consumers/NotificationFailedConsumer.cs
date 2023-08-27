using Application.UseCases.ConfirmNotification;
using Application.UseCases.FailNotification;
using Domain.Events;
using MassTransit;

namespace Workers.Consumers;

public class NotificationFailedConsumer : IConsumer<NotificationFailed>
{
    private readonly ILogger<NotificationFailedConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationFailedConsumer(ILogger<NotificationFailedConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Consume(ConsumeContext<NotificationFailed> context)
    {
        try
        {
            var notificationFailed = context.Message;
            var command = new FailNotificationCommand { NotificationId = notificationFailed.NotificationId, Errors = notificationFailed.Errors };
            
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<IFailNotificationUseCase>();

            await useCase.Execute(command);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occured at {NotificationSentConsumer}", nameof(NotificationSentConsumer));
            throw;
        }
    }
}