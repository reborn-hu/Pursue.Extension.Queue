using System.ComponentModel;

namespace Pursue.Extension.Queue
{
    public enum OverflowType
    {
        /// <summary>
        /// 删除queue头部的消息
        /// </summary>
        [Description("drop-head")]
        DropHead,

        /// <summary>
        /// 最近发来的消息将被丢弃
        /// </summary>
        [Description("reject-publish")]
        RejectPublish,

        /// <summary>
        /// 拒绝发送消息到死信交换器
        /// </summary>
        [Description("reject-publish-dlx")]
        RejectPublishDlx,
    }
}