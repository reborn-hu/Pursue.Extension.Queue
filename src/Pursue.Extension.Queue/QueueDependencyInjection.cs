using Microsoft.Extensions.DependencyInjection;
using Pursue.Extension.Queue;
using System;

namespace Pursue.Extension.DependencyInjection
{
    public static class QueueDependencyInjection
    {
        public static IServiceCollection AddQueueClient(this IServiceCollection services, Action<QueueConfigOptions> options)
        {
            if (options is null)
                throw new NullReferenceException("参数不可为空");

            options.Invoke(new QueueConfigOptions());

            services.AddSingleton<QueueConfigOptions>();
            services.AddSingleton<IRabbitMQConnectModule, RabbitMQConnectModule>();
            services.AddSingleton<IRabbitMQClient, RabbitMQClient>();

            return services;
        }
    }
}