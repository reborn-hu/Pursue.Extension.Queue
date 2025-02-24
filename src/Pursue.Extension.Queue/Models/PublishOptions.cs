using System.Text;

namespace Pursue.Extension.Queue
{
    public sealed class PublishOptions<TEntity>
    {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 自动确认
        /// -- 默认开启
        /// </summary>
        public bool AutoAck { get; set; } = true;

        /// <summary>
        /// 持久化
        /// -- 默认开启持久化
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// 有效时间
        /// -- 默认不设置
        /// </summary>
        public int? Expiration { get; set; } = null;

        /// <summary>
        /// 解析字符集
        /// -- 默认 UTF-8
        /// </summary>
        public Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 数据
        /// </summary>
        public TEntity Data { get; set; }
    }
}
