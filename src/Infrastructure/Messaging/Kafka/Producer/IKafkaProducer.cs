namespace Infrastructure.Messaging.Kafka.Producer;

public interface IKafkaProducer
{
    Task Produce(object message, string topic);
}