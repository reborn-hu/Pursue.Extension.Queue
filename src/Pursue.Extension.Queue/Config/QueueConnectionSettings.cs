using System.Collections.Generic;

namespace Pursue.Extension.Queue
{
    public sealed class QueueConnectionSettings
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Queue 连接IP端口组
        /// </summary>
        public List<QueueEndpoint> Endpoints { get; set; } = new List<QueueEndpoint>();
    }
}
