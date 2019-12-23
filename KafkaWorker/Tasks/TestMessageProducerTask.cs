using Confluent.Kafka;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace KafkaWorker.Tasks
{
    public class TestMessageProducerTask : BackgroundService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<TestMessageConsumerTask> _logger;

        public TestMessageProducerTask(ILogger<TestMessageConsumerTask> logger)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            _logger = logger;
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var deliveryResult = await _producer.ProduceAsync("hello-world-topic3", new Message<Null, string> { Value = "test" });

                _logger.LogInformation($"Produce message '{deliveryResult.Value}' at: '{deliveryResult.TopicPartitionOffset}'.");

                await Task.Delay(5000);
            }
        }
    }
}