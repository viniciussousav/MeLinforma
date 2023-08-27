using Application.Mapping;
using Application.UseCases.CreateNotification;
using Domain.Events;
using Domain.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Notifications;

[ApiController]
[Route("api/v1/notifications")]
public class NotificationsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RequestNotification(
        [FromServices] IBus bus, 
        [FromBody] NotificationRequested request)
    {
        try
        {
            await bus.Publish(request);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
}