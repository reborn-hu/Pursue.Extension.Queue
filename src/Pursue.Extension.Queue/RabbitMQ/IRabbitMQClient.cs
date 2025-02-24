using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pursue.Extension.Queue
{
    public interface IRabbitMQClient
    {
        Task CreateExchangeAsync(ExchangeOptions exchangeOptions);
        Task CreateQueueAsync(QueueCreateOptions queueOptions);
        Task ExchangeBindAsync(ExchangeBindOptions exchangeBindOptions);
        Task<QueueMessage> GetQueueMessageAsync(string queueName);
        Task PublishAsync<TEntity>(PublishOptions<TEntity> publishOptions);
        Task QueueBindAsync(QueueBindOptions queueBindOptions);
        Task ReceivedAsync<TEntity>(ReceiveOptions options, Action<AsyncEventingBasicConsumer, TEntity, Action> handler);
        Task StopAsync(CancellationTokenSource cancellationTokenSource);
    }
}