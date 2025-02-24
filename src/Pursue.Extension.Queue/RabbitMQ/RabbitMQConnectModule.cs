using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Pursue.Extension.Queue
{
    public sealed class RabbitMQConnectModule : IRabbitMQConnectModule
    {
        private readonly ILogger _logger;

        public IConnection Connection { get; private set; }

        private static readonly ManualResetEventSlim _manualResetEventSlim = new ManualResetEventSlim(false);

        public RabbitMQConnectModule(ILogger<RabbitMQConnectModule> logger)
        {
            _logger = logger;
            CreateQueueConnectAsync().Wait();
        }

        public async Task<RabbitMQConnectModule> CreateQueueConnectAsync()
        {
            try
            {
                var _connectionSettings = QueueOptions.ConnectionSettings[QueueType.RabbitMQ];

                var endpoints = _connectionSettings.Endpoints.Select(o => new AmqpTcpEndpoint
                {
                    HostName = o.Host,
                    Port = o.Port,
                    Ssl = new SslOption { Version = SslProtocols.None }
                }).ToList();

                var _factory = new ConnectionFactory
                {
                    UserName = _connectionSettings.UserName,
                    Password = _connectionSettings.Password,
                    VirtualHost = _connectionSettings.VirtualHost,

                    //ContinuationTimeout = TimeSpan.FromSeconds(500000),

                    // 断线重连
                    AutomaticRecoveryEnabled = true,
                    // 重连后恢复当前的工作进程
                    TopologyRecoveryEnabled = true
                };

                // 创建连接
                Connection = await _factory.CreateConnectionAsync(endpoints);
                // 断线监听事件
                Connection.ConnectionShutdownAsync += Connection_ShutdownAsync;

                _logger.LogInformation("{}服务连接成功!", "RabbitMQ");
            }
            // 重试下异常类型是连接相关，启用线程信号量等待重试，重试间隔3秒
            catch (Exception ex) when (ex is ConnectFailureException || ex is BrokerUnreachableException)
            {
                while (!_manualResetEventSlim.Wait(3000))
                {
                    _logger.LogWarning("正在尝试重新连接{}服务!", "RabbitMQ");
                    await CreateQueueConnectAsync();

                    _manualResetEventSlim.Set();
                }
            }
            // 其它异常终止运行
            catch (Exception ex)
            {
                _logger.LogError(ex, "发生灾难性故障,永久丢失与{}服务的连接,错误信息:{}", "RabbitMQ", ex.Message);
                _manualResetEventSlim.Dispose();
            }

            return this;
        }

        private async Task Connection_ShutdownAsync(object sender, RabbitMQ.Client.Events.ShutdownEventArgs e)
        {
            // 正常退出代码
            if (e.ReplyCode == QueueStatusCode.ExitCode)
            {
                _logger.LogInformation("状态码:{}, 客户端主动断开连接!", QueueStatusCode.ExitCode);
            }
            else
            {
                _logger.LogWarning("客户端已断开与{}服务的连接,并准备尝试重试连接!", "RabbitMQ");

                //重新连接
                await CreateQueueConnectAsync();
            }
        }
    }
}