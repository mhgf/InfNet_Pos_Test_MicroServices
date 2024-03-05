using Order.Web.Controllers.Order;
using Shared.Core.Notification;

namespace Order.Web.Controllers;

public static class ControllerConfig
{
    public static WebApplication UseControllers(this WebApplication app)
    {
        app
            .MapGroup("/api")
            .AddEndpointFilter<NotificationFilter>()
            .ConfigureOrderController();
        
        return app;
    }
}