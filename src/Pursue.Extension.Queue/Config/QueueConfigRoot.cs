using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Pursue.Extension.Queue
{
    public sealed class QueueOptions
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
        public QueueOptions UseQueueOptions(IConfiguration configuration, string configNode = "Configuration:Queue")
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
}
