namespace Workers.Consumers;

public class CreateNotificationConsumer : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Hello World");
        await Task.Delay(100000, stoppingToken); 
    }
}