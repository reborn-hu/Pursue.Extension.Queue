using System.ComponentModel;

namespace Pursue.Extension.Queue
{
    public enum QueueModeType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("default")]
        Default,

        /// <summary>
        /// 懒加载.模式下的队列会先将交换机推送过来的消息(尽可能多的)保存在磁盘上,以减少内存的占用.当消费者开始消费的时候才加载到内存中.
        /// </summary>
        [Description("lazy")]
        Lazy,
    }
}