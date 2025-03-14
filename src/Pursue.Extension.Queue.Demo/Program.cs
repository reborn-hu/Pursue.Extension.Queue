using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pursue.Extension.DependencyInjection;
using Pursue.Extension.Queue.Demo;
using System;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
try
{
    var builder = Host.CreateDefaultBuilder();

    builder.ConfigureServices(service =>
    {
        var config = new ConfigurationManager().AddJsonFile($"./appsettings.Local.json", optional: true, reloadOnChange: true).Build();

        service.AddLogging(option =>
        {
            option.AddConsole();
            option.SetMinimumLevel(LogLevel.Trace);
        });

        service.AddQueueClient(option =>
        {
            option.UseQueueOptions(config);
        });


        service.AddHostedService<QueueDemo>();
    });

    var app = builder.Build();

    app.Run();
}
catch (Exception)
{

    throw;
}

