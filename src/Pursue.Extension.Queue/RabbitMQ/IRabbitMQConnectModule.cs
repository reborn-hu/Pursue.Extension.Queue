using RabbitMQ.Client;
using System.Threading.Tasks;

namespace Pursue.Extension.Queue
{
    public interface IRabbitMQConnectModule
    {
        IConnection Connection { get; }

        Task<RabbitMQConnectModule> CreateQueueConnectAsync();
    }
}