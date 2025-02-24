namespace Pursue.Extension.Queue
{
    public sealed class QueueMessage
    {
        /// <summary>
        /// 消费者数量
        /// </summary>
        public long ConsumerCount { get; set; }

        /// <summary>
        /// 消息数量
        /// </summary>
        public long MessageCount { get; set; }
    }
}