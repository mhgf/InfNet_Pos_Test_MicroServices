using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product.Core.Repositories;
using Product.Infra.Repositories;
using Shared.Infra;

namespace Product.Infra;

public static class ProductInfraConfiguration
{
    public static IServiceCollection AddProductInfraServices(this IServiceCollection services)
    {
        services.AddDbContext<ProductContext>(options => options.UseSqlite("Data Source=Products.db"));
        services.AddScoped<IUnitOfWork>(service => service.GetRequiredService<ProductContext>());
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}