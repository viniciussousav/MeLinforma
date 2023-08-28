using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Infrastructure.Messaging.Kafka.Configuration;

namespace Infrastructure.Messaging.Kafka.Producer;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, byte[]> _producer;
    
    public KafkaProducer()
    {
        _producer = new ProducerBuilder<string, byte[]>(ProducerConfigBuilder.Build()).Build();
    }

    public async Task Produce(object message, string topic)
    {
        await _producer.ProduceAsync(topic, new Message<string, byte[]>
        {
            Value = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = false
                }))
        });
    }
}