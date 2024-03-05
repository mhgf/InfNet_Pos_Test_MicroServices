using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Core.Bus;
using Order.Core.Services;
using Order.Core.Services.Interfaces;
using Shared.Bus.Consumer;
using Shared.Bus.Producer;
using Shared.Contracts.Order;
using Shared.Contracts.Payment;
using Shared.Contracts.Product;
using Shared.Core.Notification;

namespace Order.Core;

public static class OrderCoreConfiguration
{
    public static IServiceCollection AddOrderServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<NotificationContext>();
        var hostRabbit = configuration.GetValue<string>("host_rabbit");

        if (string.IsNullOrWhiteSpace(hostRabbit)) throw new InvalidProgramException();
        services.AddSingleton<ISenderPayment>(_ =>
            (new SenderPayement(hostRabbit, "order_to_pay")));
        services.AddSingleton<ISenderProduct>(_ =>
            (new SenderProduct(hostRabbit, "product_sold")));
        
        services.AddHostedService<ConsumerMessage>(service => new ConsumerMessage(hostRabbit, "paid_order",
            (sender, e) =>
            {
                var order =  JsonSerializer.Deserialize<PaidOrderContract>(e.Body.ToArray());
                if(order is null) return;
                using var scope =  service.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                orderService.ConfirmPayment(order.OrderId, true).GetAwaiter();
            }));

        return services;
    }
}