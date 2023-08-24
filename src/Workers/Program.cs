using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using MassTransit;
using Workers.Consumers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructureDependencies(context.Configuration);
        services.AddApplicationDependencies();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationRequestedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.ConfigureEndpoints(ctx);
            });
        });
    })
    .Build();

host.Run();