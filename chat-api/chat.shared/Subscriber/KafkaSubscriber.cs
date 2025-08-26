using Chat.Shared.Configurations;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Chat.Shared.Subscriber
{
    public class KafkaSubscriber<T> : BackgroundService
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly string _bootstrapServers;
        private readonly ILogger<KafkaSubscriber<T>> _logger;
        private readonly IMediator _mediator;
        public KafkaSubscriber(IOptions<KafkaSettings> options, IHostEnvironment env, ILogger<KafkaSubscriber<T>> logger, IMediator mediator)
        {
            _kafkaSettings = options.Value;
            _bootstrapServers = env.IsDevelopment() ? _kafkaSettings.Bootstrap_Kafka_Local : _kafkaSettings.Bootstrap_Kafka_Server;
            _logger = logger;
            _mediator = mediator;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            foreach(var consumer in _kafkaSettings.Consumers)
            {
                _ = Task.Run(() => StartConsumerLoop(consumer.Key, consumer.Value, cancellationToken));
            }
            await Task.CompletedTask;
        }

        public async Task StartConsumerLoop(string key, SubscriberData value, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = value.groupid,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false, // commit manually after processing
                EnablePartitionEof = false
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).SetErrorHandler((_, e) => _logger.LogError($"Kafka error: {e.Reason}")).Build();
            consumer.Subscribe(value.topic);
            _logger.LogInformation($"Consumer: {value.consumer} , subscribed to topics: {string.Join(',', value.topic)}");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult == null || string.IsNullOrEmpty(consumeResult.Message.Value))
                            continue;

                        var message = consumeResult.Message.Value;
                        var command = JsonSerializer.Deserialize<T>(message);
                        if (command != null)
                        {
                            await _mediator.Send(command);
                            consumer.Commit(consumeResult);
                        }
                        
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Consume error in {key}: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error handling message in {key}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Consumer {key} loop canceled");
            }
            finally
            {
                consumer.Close();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
