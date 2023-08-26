using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class WebNotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has join");
    }
}