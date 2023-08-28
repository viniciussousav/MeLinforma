using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class NotificationsHub : Hub
{
    public async Task SendNotification(Guid customerId, string title, string description)
    {
        await Clients.All.SendAsync(customerId.ToString(), $"{title} - {description}");
    }
}