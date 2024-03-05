using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Core.Repositories;
using Order.Core.Services.Http;
using Order.Infra.Repositories;
using Order.Infra.Services;
using Shared.Infra;

namespace Order.Infra;

public static class OrderInfraConfiguration
{
    public static IServiceCollection AddOrderInfraServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<OrderContext>(options => options.UseSqlite("Data Source=Orders.db"));
        services.AddScoped<IUnitOfWork>(service => service.GetRequiredService<OrderContext>());
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductService>(_ =>
        {
            var url = configuration.GetValue<string>("product_url");
            if (string.IsNullOrEmpty(url)) throw new InvalidConstraintException();

            return new ProductServices(url);
        });
        return services;
    }
}