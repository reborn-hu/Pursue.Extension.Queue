namespace Pursue.Extension.Queue
{
    public sealed class QueueBindOptions
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; private set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange { get; private set; }

        /// <summary>
        /// 用来创建绑定关系的路由键
        /// </summary>
        public string RoutingKey { get; private set; }


        public QueueBindOptions(string queue, string exchange)
        {
            Queue = queue;
            Exchange = exchange;
            RoutingKey = null;
        }

        public QueueBindOptions(string queue, string exchange, string routingKey)
        {
            Queue = queue;
            Exchange = exchange;
            RoutingKey = routingKey;
        }
    }
}