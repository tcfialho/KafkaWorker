using KafkaProducer.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaProducer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<TestMessageProducerTask>();

            //services.AddMessageQueueKafka(setup =>
            //{
            //    var options = Configuration.GetSection("Kafka").Get<MessageQueueOptions>();

            //    setup.Host = options.Host;
            //    setup.Port = options.Port;
            //    setup.ExchangeName = options.ExchangeName;
            //    setup.QueueName = options.QueueName;
            //    setup.Username = options.Username;
            //    setup.Password = options.Password;
            //    setup.CreateIfNotExists = true;
            //});

            //services.AddHealthChecks()
            //        .AddKafkaCheck<TestMessage>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHealthChecks("/hc");
        }
    }
}
