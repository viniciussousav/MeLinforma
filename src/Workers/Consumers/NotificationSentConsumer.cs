using Application.UseCases.ConfirmNotification;
using Domain.Events;
using MassTransit;

namespace Workers.Consumers;

public class NotificationSentConsumer : IConsumer<NotificationSent>
{
    private readonly ILogger<NotificationSentConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationSentConsumer(ILogger<NotificationSentConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Consume(ConsumeContext<NotificationSent> context)
    {
        try
        {
            var notificationSent = context.Message;
            var command = new ConfirmNotificationCommand { NotificationId = notificationSent.NotificationId};
            
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<IConfirmNotificationUseCase>();

            await useCase.Execute(command);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occured at {NotificationSentConsumer}", nameof(NotificationSentConsumer));
            throw;
        }
    }
}