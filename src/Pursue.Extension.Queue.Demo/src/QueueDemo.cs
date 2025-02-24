using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Pursue.Extension.Queue.Demo
{
    public class QueueDemo : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IRabbitMQClient _rabbitMQClient;

        public QueueDemo(ILogger<QueueDemo> logger, IRabbitMQClient rabbitMQClient)
        {
            _logger = logger;
            _rabbitMQClient = rabbitMQClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _rabbitMQClient.CreateExchangeAsync(new ExchangeOptions("Exc_Demo", ExchangeSchemaType.Direct, true, false));
            await _rabbitMQClient.CreateQueueAsync(new QueueCreateOptions("Que_Demo_1", true, false, false));
            await _rabbitMQClient.QueueBindAsync(new QueueBindOptions("Que_Demo_1", "Exc_Demo"));

            var receiveOptions = new ReceiveOptions
            {
                Queue = "Que_Demo_1",
                AutoAck = false,
            };

            await _rabbitMQClient.ReceivedAsync<string>(receiveOptions, (consumer, msg, ack) =>
            {
                _logger.LogInformation(msg);
                ack();
                Thread.Sleep(3000);
            });

            //for (int i = 0; i < 100; i++)
            //{
            //    var msg = new PublishOptions<string>
            //    {
            //        Exchange = "Exc_Demo",
            //        AutoAck = false,
            //        Durable = true,
            //        Data = $"测试一下-{i}"
            //    };
            //    await _rabbitMQClient.PublishAsync(msg);
            //    //Thread.Sleep(2000);
            //}
        }
    }
}
