using Application.DependencyInjection;
using Infrastructure.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructureDependencies(context.Configuration);
        services.AddApplicationDependencies();
        services.AddHostedService<BackgroundService>();
    })
    .Build();

host.Run();