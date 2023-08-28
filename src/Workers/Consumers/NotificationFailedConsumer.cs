using Application.UseCases.ConfirmNotification;
using Application.UseCases.FailNotification;
using Domain.Events;
using Infrastructure.Messaging.Kafka.Constants;
using Infrastructure.Messaging.Kafka.Producer;
using MassTransit;

namespace Workers.Consumers;

public class NotificationFailedConsumer : IConsumer<NotificationFailed>
{
    private readonly ILogger<NotificationFailedConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IKafkaProducer _kafkaProducer;

    public NotificationFailedConsumer(ILogger<NotificationFailedConsumer> logger, IServiceScopeFactory serviceScopeFactory, IKafkaProducer kafkaProducer)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _kafkaProducer = kafkaProducer;
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
            
            await NotifyProducts(notificationFailed);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occured at {NotificationSentConsumer}", nameof(NotificationSentConsumer));
            throw;
        }
    }

    private async Task NotifyProducts(NotificationFailed notificationFailed)
    {
        try
        {
            await _kafkaProducer.Produce(notificationFailed, Topics.NotificationFailed);
            _logger.LogInformation("Notification {NotificationId} failed event sent to {NotificationFailed}",
                notificationFailed.NotificationId, Topics.NotificationFailed);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending Notification {NotificationId} failed event sent to {NotificationFailed}",
                notificationFailed.NotificationId, Topics.NotificationFailed);
        }
    }
}