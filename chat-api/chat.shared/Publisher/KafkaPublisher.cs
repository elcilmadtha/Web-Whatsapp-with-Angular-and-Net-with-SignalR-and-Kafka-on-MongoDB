using Chat.Shared.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Chat.Shared.Publisher
{
    public class KafkaPublisher: IKafkaPublisher
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly string _bootstrapServer;
        public KafkaPublisher(IOptions<KafkaSettings> options, IHostEnvironment env) {
            _kafkaSettings = options.Value;
            _bootstrapServer = env.IsDevelopment() ? _kafkaSettings.Bootstrap_Kafka_Local : _kafkaSettings.Bootstrap_Kafka_Server;
        }

        public async Task PublishAsync<T>(string topicKey, T message)
        {
            if (!_kafkaSettings.Topics.ContainsKey(topicKey))
                throw new ArgumentException($"Topic '{topicKey}' not found in configuration.");

            var topicName = _kafkaSettings.Topics[topicKey].topic;

            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServer,
                Debug = "all",    
                MessageTimeoutMs = 5000, 
                SocketTimeoutMs = 5000
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var jsonMessage = JsonSerializer.Serialize(message);

            var kafkaMessage = new Message<Null, string> { Value = jsonMessage };

            var deliveryResult = await producer.ProduceAsync(topicName, kafkaMessage);

            Console.WriteLine($"Message published to {deliveryResult.TopicPartitionOffset}");
        }
    }
}
