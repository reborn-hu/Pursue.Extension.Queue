# Pursue.Extension.Queue

## 项目简介
Pursue.Extension.Queue 是Pursue系列队列操作库，支持.NET6+，支持 RabbitMQ 的集成和使用。提供了便捷的依赖注入、队列创建、消息发布和接收等功能。

## 功能特性

- **提供模板化实现**: 可以持续集成更多队列的支持。
- **灵活的配置**: 提供灵活的配置选项，满足不同应用场景的需求。
- **易于扩展**: 设计良好的接口，方便开发者扩展和定制功能。

## 安装
可以通过 NuGet 包管理器安装此库：

```sh
dotnet add package Pursue.Extension.Queue
```

## 快速开始
以下是一个简单的使用示例：

### 1. 配置项目
在 `appsettings.Local.json` 文件中配置 RabbitMQ 的连接设置：

```json

{
    "Configuration": {
        "Queue": {
            "Enable": true,
            "ConnectionSettings": {
                "RabbitMQ": {
                    "UserName": "rabbitmq 账号",
                    "Password": "rabbitmq 密码",
                    "VirtualHost": "rabbitmq VHost",
                    "Endpoints": [
                        {
                            "Host": "rabbitmq 服务地址",
                            "Port": "5672"
                        }
                    ]
                }
            }
        }
    }
}

```

### 2. 配置服务
在 `Program.cs` 中配置服务：

```csharp

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(service => { 
    
    var config = new ConfigurationManager()
    .AddJsonFile($"./appsettings.Local.json", optional: true, reloadOnChange: true)
    .Build();

    service.AddLogging(option =>
    {
        option.AddConsole();
        option.SetMinimumLevel(LogLevel.Trace);
    });
  
    // 注入 RabbitMQ 队列服务，并使用配置初始化
    service.AddQueueClient(option =>
    {
        option.UseQueueOptions(config);
    });

    // 注入后台服务 QueueDemo，演示队列操作
    service.AddHostedService<QueueDemo>();

    var app = builder.Build(); app.Run();

```

### 3. 创建队列和接收消息
在 `QueueDemo.cs` 中创建队列并接收消息：

```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
     await _rabbitMQClient.CreateExchangeAsync(new ExchangeOptions("Exc_Demo", ExchangeSchemaType.Direct, true, false)); 
     await _rabbitMQClient.CreateQueueAsync(new QueueCreateOptions("Que_Demo_1", true, false, false)); 
     await _rabbitMQClient.QueueBindAsync(new QueueBindOptions("Que_Demo_1", "Exc_Demo"));
    
    var receiveOptions = new ReceiveOptions
    {
        Queue = "Que_Demo_1",
        AutoAck = false,
    };

    await _rabbitMQClient.ReceivedAsync<string>(receiveOptions, (consumer, msg, ack) =>
    {
        _logger.LogInformation(msg);
        ack();
        Thread.Sleep(3000);
    });

    //for (int i = 0; i < 100; i++)
    //{
    //    var msg = new PublishOptions<string>
    //    {
    //        Exchange = "Exc_Demo",
    //        AutoAck = false,
    //        Durable = true,
    //        Data = $"测试一下-{i}"
    //    };
    //    await _rabbitMQClient.PublishAsync(msg);
    //    Thread.Sleep(2000);
    //}
}
```