using System.ComponentModel;

namespace Pursue.Extension.Queue
{
    public enum MasterLocatorType
    {
        /// <summary>
        /// 选择承载了队列master数量最少的节点
        /// </summary>
        [Description("min-masters")]
        MinMasters,

        /// <summary>
        ///  选择客户端声明队列时连接上的那个节点
        /// </summary>
        [Description("client-local")]
        ClientLocal,

        /// <summary>
        /// 随机选择一个节点
        /// </summary>
        [Description("random")]
        Random,
    }
}