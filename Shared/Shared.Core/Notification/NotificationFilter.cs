using Microsoft.AspNetCore.Http;

namespace Shared.Core.Notification;


public class NotificationFilter(NotificationContext notificationContext) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result =  await next(context);
        
        return !notificationContext.HasNotifications ? result : TypedResults.BadRequest(notificationContext.Notifications);
    }
}