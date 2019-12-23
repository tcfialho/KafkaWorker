using Confluent.Kafka;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace KafkaWorker.Tasks
{
    public class TestMessageConsumerTask : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<TestMessageConsumerTask> _logger;

        public TestMessageConsumerTask(ILogger<TestMessageConsumerTask> logger)
        {
            var config = new ConsumerConfig
            {
                GroupId = "hello-world-consumer",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _logger = logger;
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe("hello-world-topic3");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                _consumer.Commit();

                _logger.LogInformation($"Consumed message '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
            }
        }
    }
}