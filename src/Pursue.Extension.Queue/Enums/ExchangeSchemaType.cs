using System.ComponentModel;

namespace Pursue.Extension.Queue
{
    public enum ExchangeSchemaType
    {
        /// <summary>
        /// 定向交换机也叫点对点交换机
        /// </summary>
        [Description("direct")]
        Direct,

        /// <summary>
        /// 定向交换机也叫点对点交换机
        /// </summary>
        [Description("headers")]
        Headers,

        /// <summary>
        /// 广播交换机
        /// </summary>
        [Description("fanout")]
        Fanout,

        /// <summary>
        /// 主题交换机
        /// </summary>
        [Description("topic")]
        Topic
    }
}