using System.Collections.Generic;

namespace Pursue.Extension.Queue
{
    public sealed class QueueCreateOptions
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; private set; }

        /// <summary>
        /// 持久化
        /// -- 默认开启
        /// </summary>
        public bool Durable { get; private set; }

        /// <summary>
        /// 是否是排他队列
        /// 客户端断开连接后自动删除该队列
        /// </summary>
        public bool Exclusive { get; private set; }

        /// <summary>
        /// 当最后一个消费者断开连接之后队列是否自动被删除，可以通过RabbitMQ Management，查看某个队列的消费者数量，当consumers = 0时队列就会自动删除.
        /// --false 默认关闭
        /// </summary>
        public bool AutoDelete { get; private set; }

        /// <summary>
        /// 额外参数
        /// </summary>
        public IDictionary<string, object> Arguments { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName">队列名称</param>
        public QueueCreateOptions(string queueName)
        {
            Queue = queueName;
            Durable = false;
            Exclusive = true;
            AutoDelete = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="durable">持久化</param>
        public QueueCreateOptions(string queueName, bool durable)
        {
            Queue = queueName;
            Durable = durable;
            Exclusive = true;
            AutoDelete = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="durable">持久化</param>
        /// <param name="exclusive">是否是排他队列</param>
        public QueueCreateOptions(string queueName, bool durable, bool exclusive)
        {
            Queue = queueName;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="durable">持久化</param>
        /// <param name="exclusive">是否是排他队列</param>
        /// <param name="autoDelete"> 当最后一个消费者断开连接之后队列是否自动被删除，可以通过RabbitMQ Management，查看某个队列的消费者数量，当consumers = 0时队列就会自动删除.</param>
        public QueueCreateOptions(string queueName, bool durable, bool exclusive, bool autoDelete)
        {
            Queue = queueName;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
        }


        /// <summary>
        /// 队列中消息的过期时间,发送到队列的消息可以存活多长时间. 
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public QueueCreateOptions SetMessageTTL(int second)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-message-ttl"))
                    Arguments["x-message-ttl"] = second * 1000;
                else
                    Arguments.Add("x-message-ttl", second * 1000);
            }

            return this;
        }

        /// <summary>
        /// 队列的过期时间,队列在被自动删除之前可以存活多长时间. 
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public QueueCreateOptions SetExpires(int second)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-expires"))
                    Arguments["x-expires"] = second * 1000;
                else
                    Arguments.Add("x-expires", second * 1000);
            }

            return this;
        }

        /// <summary>
        /// 队列最多可以容纳多少条消息,超出部分从头部开始删除.队列中可以存储处于ready状态的消息数量
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public QueueCreateOptions SetMaxLength(int count)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-max-length"))
                    Arguments["x-max-length"] = count;
                else
                    Arguments.Add("x-max-length", count);
            }

            return this;
        }

        /// <summary>
        /// 队列中可以存储处于ready状态的消息占用的内存空间,单位:字节
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public QueueCreateOptions SetMaxLengthBytes(int size)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-max-length-bytes"))
                    Arguments["x-max-length"] = size;
                else
                    Arguments.Add("x-max-length", size);
            }

            return this;
        }

        /// <summary>
        /// 队列溢出行为,这将决定当队列达到设置的最大长度或者最大的存储空间时发送到消息队列的消息的处理方式
        /// </summary>
        /// <param name="overflowType"></param>
        /// <returns></returns>
        public QueueCreateOptions SetOverflow(OverflowType overflowType)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-overflow"))
                    Arguments["x-overflow"] = overflowType.GetDescription();
                else
                    Arguments.Add("x-overflow", overflowType.GetDescription());
            }

            return this;
        }

        /// <summary>
        /// 设置死信交换机的名称
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public QueueCreateOptions SetDeadLetterExchange(string exchange)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-dead-letter-exchange"))
                    Arguments["x-dead-letter-exchange"] = exchange;
                else
                    Arguments.Add("x-dead-letter-exchange", exchange);
            }

            return this;
        }

        /// <summary>
        /// 设置死信交换机的路由键名称.如果未设置,将使用消息的原始路由密钥.
        /// </summary>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public QueueCreateOptions SetDeadLetterRoutingKey(string routingKey)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-dead-letter-routing-key"))
                    Arguments["x-dead-letter-routing-key"] = routingKey;
                else
                    Arguments.Add("x-dead-letter-routing-key", routingKey);
            }

            return this;
        }

        /// <summary>
        /// 设置该队列中的消息的优先级最大值.发布消息的时候,可以指定消息的优先级,优先级高的先被消费.如果没有设置该参数,那么该队列不支持消息优先级功能.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public QueueCreateOptions SetMaxPriority(int num)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-max-priority"))
                    Arguments["x-max-priority"] = num;
                else
                    Arguments.Add("x-max-priority", num);
            }

            return this;
        }

        /// <summary>
        /// 设置队列模式
        /// </summary>
        /// <param name="queueModeType"></param>
        /// <returns></returns>
        public QueueCreateOptions SetQueueMode(QueueModeType queueModeType)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-queue-mode"))
                    Arguments["x-queue-mode"] = queueModeType.GetDescription();
                else
                    Arguments.Add("x-queue-mode", queueModeType.GetDescription());
            }

            return this;
        }

        /// <summary>
        /// 将队列设置为主位置模式,确定在节点集群上声明时队列主机所在的规则.
        /// </summary>
        /// <returns></returns>
        public QueueCreateOptions SetQueueMasterLocator(MasterLocatorType masterLocatorType)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("x-queue-master-locator"))
                    Arguments["x-queue-master-locator"] = masterLocatorType.GetDescription();
                else
                    Arguments.Add("x-queue-master-locator", masterLocatorType.GetDescription());
            }

            return this;
        }
    }
}