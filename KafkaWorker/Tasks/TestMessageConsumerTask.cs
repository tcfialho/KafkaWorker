using Confluent.Kafka;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaWorker.Tasks
{
    public class TestMessageConsumerTask : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<TestMessageConsumerTask> _logger;

        public TestMessageConsumerTask(ILogger<TestMessageConsumerTask> logger)
        {
            _logger = logger;

            _consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            {
                GroupId = "Test",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false
            }).Build();

            _consumer.Subscribe("Test");

            _producer = new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            }).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    _consumer.StoreOffset(consumeResult);

                    _logger.LogInformation($"Consumed message '{consumeResult.Value}' at: '{consumeResult.Topic}' - '{consumeResult.TopicPartitionOffset}'.");
                }
                catch (Exception)
                {
                    await _producer.ProduceAsync("Test-Retry", new Message<Null, string> { Value = "test" });
                    throw;
                }
            }
        }
    }
}