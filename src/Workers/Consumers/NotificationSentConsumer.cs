using Application.UseCases.ConfirmNotification;
using Domain.Events;
using Infrastructure.Messaging.Kafka.Constants;
using Infrastructure.Messaging.Kafka.Producer;
using MassTransit;

namespace Workers.Consumers;

public class NotificationSentConsumer : IConsumer<NotificationSent>
{
    private readonly ILogger<NotificationSentConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IKafkaProducer _kafkaProducer;

    public NotificationSentConsumer(ILogger<NotificationSentConsumer> logger, IServiceScopeFactory serviceScopeFactory, IKafkaProducer kafkaProducer)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Consume(ConsumeContext<NotificationSent> context)
    {
        try
        {
            var notificationSent = context.Message;
            var command = new ConfirmNotificationCommand(notificationSent.NotificationId);
            
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<IConfirmNotificationUseCase>();

            await useCase.Execute(command);

            await NotifyProducts(notificationSent);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception occured at {NotificationSentConsumer}", nameof(NotificationSentConsumer));
            throw;
        }
    }
    
    private async Task NotifyProducts(NotificationSent notificationSent)
    {
        try
        {
            await _kafkaProducer.Produce(notificationSent, Topics.NotificationConfirmed);
            _logger.LogInformation("Notification {NotificationId} confirmed event sent to {NotificationConfirmed}",
                notificationSent.NotificationId, Topics.NotificationConfirmed);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending Notification {NotificationId} confirmed event sent to {NotificationConfirmed}",
                notificationSent.NotificationId, Topics.NotificationConfirmed);
        }
    }
}