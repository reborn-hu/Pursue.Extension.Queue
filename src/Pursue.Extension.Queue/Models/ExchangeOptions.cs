using System.Collections.Generic;

namespace Pursue.Extension.Queue
{
    public sealed class ExchangeOptions
    {
        /// <summary>
        ///  交换机名称
        /// </summary>
        public string Exchange { get; private set; }

        /// <summary>
        /// 交换机模式
        /// </summary>
        public string ExchangeSchema { get; private set; }

        /// <summary>
        /// 持久化
        /// --true 默认开启
        /// </summary>
        public bool Durable { get; private set; }

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
        /// <param name="exchangeName">交换机名称</param>
        public ExchangeOptions(string exchangeName)
        {
            Exchange = exchangeName;
            ExchangeSchema = "";
            Durable = true;
            AutoDelete = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeSchemaType">交换机模式</param>
        public ExchangeOptions(string exchangeName, ExchangeSchemaType exchangeSchemaType)
        {
            Exchange = exchangeName;
            ExchangeSchema = exchangeSchemaType.GetDescription();
            Durable = true;
            AutoDelete = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeSchemaType">交换机模式</param>
        /// <param name="durable">持久化 --true 默认开启</param>
        public ExchangeOptions(string exchangeName, ExchangeSchemaType exchangeSchemaType, bool durable)
        {
            Exchange = exchangeName;
            ExchangeSchema = exchangeSchemaType.GetDescription();
            Durable = durable;
            AutoDelete = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeSchemaType">交换机模式</param>
        /// <param name="durable">持久化 --true 默认开启</param>
        /// <param name="autoDelete">是否自动删除绑定 --false 默认关闭</param>
        public ExchangeOptions(string exchangeName, ExchangeSchemaType exchangeSchemaType, bool durable, bool autoDelete)
        {
            Exchange = exchangeName;
            ExchangeSchema = exchangeSchemaType.GetDescription();
            Durable = durable;
            AutoDelete = autoDelete;
        }

        /// <summary>
        /// 添加备份交换机
        /// </summary>
        /// <param name="alternateExchange">备份交换机名称</param>
        /// <returns></returns>
        public ExchangeOptions SetAlternateExchange(string alternateExchange)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<string, object>();
            }
            else
            {
                if (Arguments.ContainsKey("alternate-exchange"))
                    Arguments["alternate-exchange"] = alternateExchange;
                else
                    Arguments.Add("alternate-exchange", alternateExchange);
            }
            return this;
        }
    }
}