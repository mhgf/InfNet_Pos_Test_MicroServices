using Shared.Core.Notification;

namespace User.Web.Controllers;

public static class ConfigureControllers
{
    public static WebApplication UseControllers(this WebApplication app)
    {
        app
            .MapGroup("/api")
            .AddEndpointFilter<NotificationFilter>()
            .ConfigureUserController();
        
        return app;
    }
}