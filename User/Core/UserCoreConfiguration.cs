using Core.Services;
using Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Notification;

namespace Core;

public static class UserCoreConfiguration
{
    public static IServiceCollection AddCoreConfiguration(this IServiceCollection service)
    {
        service.AddScoped<NotificationContext>();
        service.AddScoped<IUserServices, UserServices>();
        
        return service;
    }
}