using RabbitMQ.Client;

namespace Pursue.Extension.Queue
{
    public sealed class ChannelMonitor
    {
        public ReceiveOptions Options { get; set; }

        public IChannel Channel { get; set; }
    }
}
