using Confluent.Kafka;

namespace Infrastructure.Messaging.Kafka.Configuration;

public static class ProducerConfigBuilder
{
    public static ProducerConfig Build()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        return config;
    }
}