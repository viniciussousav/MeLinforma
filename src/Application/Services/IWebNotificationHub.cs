namespace Application.Services;

public interface IWebNotificationHub
{
    Task TryNotifyNow(Guid customerId, string title, string description);
}