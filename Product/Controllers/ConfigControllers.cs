using Product.web.Controllers.Product;
using Shared.Core.Notification;

namespace Product.web.Controllers;

public static class ConfigControllers
{
    public static WebApplication UseControllers(this WebApplication app)
    {
        app
            .MapGroup("/api")
            .AddEndpointFilter<NotificationFilter>()
            .ConfigureProductController();
        
        return app;
    }
}