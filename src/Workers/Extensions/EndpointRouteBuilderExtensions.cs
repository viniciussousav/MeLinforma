using Application.Services;

namespace Workers.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void AddHubs(this IEndpointRouteBuilder builder)
    {
        builder.MapHub<NotificationsHub>("notifications");
    }
}