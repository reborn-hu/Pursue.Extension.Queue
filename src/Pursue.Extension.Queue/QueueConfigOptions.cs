using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Pursue.Extension.Queue
{
    public sealed class QueueConfigOptions
    {
        /// <summary>
        /// Queue是否启用
        /// -- 默认: false
        /// </summary>
        public static bool Enable { get; private set; } = false;

        /// <summary>
        /// 连接参数字典
        /// </summary>
        public static ConcurrentDictionary<QueueType, QueueConnectionSettings> ConnectionSettings { get; private set; }

        /// <summary>
        /// 注入配置
        /// </summary>
        /// <returns></returns>
        public QueueConfigOptions UseQueueOptions(IConfiguration configuration, string configNode = "Configuration:Queue")
        {
            var config = configuration.GetSection(configNode).Get<QueueConfigRoot>();

            if (config.Enable)
            {
                Enable = config.Enable;
                ConnectionSettings = config.ConnectionSettings;
            }

            return this;
        }
    }

    public sealed class QueueConfigRoot
    {
        /// <summary>
        /// Queue是否启用
        /// -- 默认: false
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 连接参数字典
        /// </summary>
        public ConcurrentDictionary<QueueType, QueueConnectionSettings> ConnectionSettings { get; set; }
    }

    public sealed class QueueConnectionSettings
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Queue 连接IP端口组
        /// </summary>
        public List<QueueEndpoint> Endpoints { get; set; } = new List<QueueEndpoint>();
    }

    public sealed class QueueEndpoint
    {
        /// <summary>
        /// IP
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
    }

    public sealed class QueueStatusCode
    {
        public const ushort ExitCode = 131;
    }

    public enum QueueType
    {
        RabbitMQ
    }
}
