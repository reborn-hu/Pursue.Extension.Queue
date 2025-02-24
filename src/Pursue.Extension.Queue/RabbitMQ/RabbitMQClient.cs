using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pursue.Extension.Queue
{
    public sealed class RabbitMQClient : IRabbitMQClient
    {
        private readonly ILogger _logger;

        private readonly IConnection _connection;

        private readonly static ConcurrentDictionary<CancellationTokenSource, ChannelMonitor> _instances = new ConcurrentDictionary<CancellationTokenSource, ChannelMonitor>();

        public RabbitMQClient(ILogger<RabbitMQClient> logger, IRabbitMQConnectModule connect)
        {
            _logger = logger;
            _connection = connect.Connection;
        }

        public async Task<QueueMessage> GetQueueMessageAsync(string queueName)
        {
            using (var channel = await _connection.CreateChannelAsync())
            {
                if (!string.IsNullOrEmpty(queueName))
                {
                    try
                    {
                        // 获取队列信息
                        var queue = await channel.QueueDeclarePassiveAsync(queueName);
                        if (queue != null)
                        {
                            return new QueueMessage { ConsumerCount = queue.ConsumerCount, MessageCount = queue.MessageCount };
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"队列:{queueName} 查询数据异常！");
                        return new QueueMessage { ConsumerCount = 0, MessageCount = 0 };
                    }
                }
                return new QueueMessage { ConsumerCount = 0, MessageCount = 0 };
            }
        }

        public async Task CreateExchangeAsync(ExchangeOptions exchangeOptions)
        {
            if (exchangeOptions != null)
            {
                using (var channel = await _connection.CreateChannelAsync())
                {
                    await channel.ExchangeDeclareAsync(exchangeOptions.Exchange, exchangeOptions.ExchangeSchema, exchangeOptions.Durable, exchangeOptions.AutoDelete, exchangeOptions.Arguments);
                }
            }
        }

        public async Task CreateQueueAsync(QueueCreateOptions queueOptions)
        {
            if (queueOptions != null)
            {
                using (var channel = await _connection.CreateChannelAsync())
                {
                    await channel.QueueDeclareAsync(queueOptions.Queue, queueOptions.Durable, queueOptions.Exclusive, queueOptions.AutoDelete, queueOptions.Arguments);
                }
            }
        }

        public async Task ExchangeBindAsync(ExchangeBindOptions exchangeBindOptions)
        {
            if (exchangeBindOptions != null)
            {
                using (var channel = await _connection.CreateChannelAsync())
                {
                    await channel.ExchangeBindAsync(exchangeBindOptions.Destination, exchangeBindOptions.Source, exchangeBindOptions.RoutingKey);
                }
            }
        }

        public async Task QueueBindAsync(QueueBindOptions queueBindOptions)
        {
            if (queueBindOptions != null)
            {
                using (var channel = await _connection.CreateChannelAsync())
                {
                    await channel.QueueBindAsync(queueBindOptions.Queue, queueBindOptions.Exchange, queueBindOptions.RoutingKey);
                }
            }
        }

        public async Task PublishAsync<TEntity>(PublishOptions<TEntity> publishOptions)
        {
            using (var channel = await _connection.CreateChannelAsync())
            {
                var body = publishOptions.Encoding.GetBytes(JsonConvert.SerializeObject(publishOptions.Data));

                var property = new BasicProperties
                {
                    // 持久化
                    DeliveryMode = publishOptions.Durable ? DeliveryModes.Persistent : DeliveryModes.Transient
                };

                // 保留时间
                if (publishOptions.Expiration.GetValueOrDefault() > 0)
                {
                    property.Expiration = publishOptions.Expiration.ToString();
                }

                // 直接推送队列
                if (string.IsNullOrEmpty(publishOptions.Exchange) && !string.IsNullOrEmpty(publishOptions.Queue))
                {
                    publishOptions.Exchange = "";
                    publishOptions.RoutingKey = publishOptions.Queue;
                }

                await channel.BasicPublishAsync(exchange: publishOptions.Exchange, routingKey: publishOptions.RoutingKey, mandatory: false, basicProperties: property, body: body);
            }
        }

        public Task ReceivedAsync<TEntity>(ReceiveOptions options, Action<AsyncEventingBasicConsumer, TEntity, Action> handler)
        {
            try
            {
                var action = new Action(async () =>
                {
                    using (var channel = await _connection.CreateChannelAsync())
                    {
                        if (options.AutoAck == false)
                        {
                            await channel.BasicQosAsync(prefetchSize: options.PrefetchSize, prefetchCount: options.PrefetchCount, global: false);
                        }

                        var consumer = new AsyncEventingBasicConsumer(channel);
                        consumer.ReceivedAsync += async (model, msg) =>
                        {
                            TEntity message = default;
                            try
                            {
                                message = JsonConvert.DeserializeObject<TEntity>(Encoding.UTF8.GetString(msg.Body.ToArray()));

                                handler(model as AsyncEventingBasicConsumer, message, async () =>
                                {
                                    if (options.AutoAck == false)
                                    {
                                        await channel.BasicAckAsync(msg.DeliveryTag, false);
                                    }
                                });

                                _logger.LogDebug("队列:{}, 状态:'消费成功!', 报文:{}", options.Queue, JsonConvert.SerializeObject(message));
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "队列:{}, 状态:'消费失败!', 报文:{}", options.Queue, JsonConvert.SerializeObject(message));
                            }

                            await Task.CompletedTask;
                        };

                        await channel.BasicConsumeAsync(queue: options.Queue, autoAck: options.AutoAck, consumer: consumer);
                        _logger.LogInformation("队列名称:{}, 状态:订阅成功!", options.Queue);

                        if (options.CancellationTokenSource != null)
                        {
                            _instances.TryAdd(options.CancellationTokenSource, new ChannelMonitor { Channel = channel, Options = options });
                            _logger.LogInformation("队列名称:{}, 状态:状态监听添加成功!", options.Queue);
                        }

                        while (true)
                        {
                            if (channel.IsOpen)
                            {
                                _logger.LogInformation($"RabbitMQ正常运行中, 当前时间:{DateTime.Now}, 当前线程:{Thread.CurrentThread.ManagedThreadId}");
                                Thread.Sleep(300000);
                            }
                            else
                            {
                                _logger.LogInformation($"RabbitMQ意外终止, 尝试重试中... 当前时间:{DateTime.Now}, 当前线程:{Thread.CurrentThread.ManagedThreadId}");
                                break;
                            }
                        }
                        _logger.LogInformation("队列名称:{}, 状态:正在停止...", options.Queue);
                    }
                });

                if (options.CancellationTokenSource != null)
                {
                    return Task.Factory.StartNew(action, options.CancellationTokenSource.Token, TaskCreationOptions.AttachedToParent, TaskScheduler.Default);
                }
                else
                {
                    return Task.Factory.StartNew(action, TaskCreationOptions.AttachedToParent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "队列名称:{}, 启动异常!", options.Queue);
                return Task.CompletedTask;
            }
        }

        public async Task StopAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_instances.TryGetValue(cancellationTokenSource, out ChannelMonitor channelMonitor))
            {
                await channelMonitor.Channel.CloseAsync(QueueStatusCode.ExitCode, $"已关闭:{channelMonitor.Options.Queue}Channel!");
                _logger.LogWarning("Queue:【{}】的Channel已关闭!", channelMonitor.Options.Queue);
            }
        }
    }
}