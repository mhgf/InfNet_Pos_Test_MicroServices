using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Core.Services;
using Product.Core.Services.Interfaces;
using Shared.Bus.Consumer;
using Shared.Contracts.Product;
using Shared.Core.Notification;

namespace Product.Core;

public static class ProductCoreConfiguration
{
    public static IServiceCollection AddProductCoreConfiguration(this IServiceCollection service,
        ConfigurationManager configuration)
    {
        service.AddScoped<NotificationContext>();
        service.AddScoped<IProductServices, ProductService>();
        var hostRabbit = configuration.GetValue<string>("host_rabbit");

        if (string.IsNullOrWhiteSpace(hostRabbit)) throw new InvalidProgramException();

        service.AddHostedService<ConsumerMessage>(services => new ConsumerMessage(hostRabbit, "product_sold",
            (sender, e) =>
            {
                var product = JsonSerializer.Deserialize<ProductSoldContract>(e.Body.ToArray());
                if (product is null) return;
                using var scope = services.CreateScope();
                var productServices = scope.ServiceProvider.GetRequiredService<IProductServices>();
                foreach (var item in product.Items)
                {
                    productServices.RemoveInStock(item.ProductId, item.Quantity).GetAwaiter();
                }
            }));


        return service;
    }
}