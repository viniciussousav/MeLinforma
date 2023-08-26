namespace Application.Services;

public interface IWebNotificationHub
{
    Task Execute(Guid customerId, string title, string description);
}