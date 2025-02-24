namespace Pursue.Extension.Queue
{
    public sealed class ExchangeBindOptions
    {
        /// <summary>
        /// 源交换机名称
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// 目标交换机名称
        /// </summary>
        public string Destination { get; private set; }

        /// <summary>
        /// 用来创建绑定关系的路由键
        /// </summary>
        public string RoutingKey { get; private set; }

        public ExchangeBindOptions(string source, string destination, string routingKey)
        {
            Source = source; ;
            Destination = destination;
            RoutingKey = routingKey;
        }
    }
}