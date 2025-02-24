using System.Collections.Generic;
using System.Threading;

namespace Pursue.Extension.Queue
{
    public sealed class ReceiveOptions
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// 预取值大小
        /// </summary>
        public uint PrefetchSize { get; set; } = 0;

        /// <summary>
        /// Qos一次出队数
        /// </summary>
        public ushort PrefetchCount { get; set; } = 1;

        /// <summary>
        /// 自动确认
        /// -- 默认开启
        /// </summary>
        public bool AutoAck { get; set; } = true;

        /// <summary>
        /// 参数字典
        /// </summary>
        public Dictionary<string, object> Arguments { get; set; } = null;

        /// <summary>
        /// 线程取消标记
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}