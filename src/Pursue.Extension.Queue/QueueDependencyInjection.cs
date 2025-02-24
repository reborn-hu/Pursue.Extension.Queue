using Microsoft.Extensions.DependencyInjection;
using System;

namespace Pursue.Extension.Queue.DependencyInjection
{
    public static class QueueDependencyInjection
    {
        public static IServiceCollection AddQueueClient(this IServiceCollection services, Action<QueueOptions> options)
        {
            if (options is null)
                throw new NullReferenceException("参数不可为空");

            options.Invoke(new QueueOptions());

            services
                .AddSingleton<QueueOptions>()
                .AddSingleton<IRabbitMQConnectModule, RabbitMQConnectModule>()
                .AddSingleton<IRabbitMQClient, RabbitMQClient>();

            return services;
        }
    }
}