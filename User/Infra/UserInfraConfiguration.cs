using Core.Repositories;
using Infra.Repostitories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infra;

namespace Infra;

public static class UserInfraConfiguration
{
    public static IServiceCollection AddUserInfraServices(this IServiceCollection services)
    {
        services.AddDbContext<UserContext>(options => options.UseSqlite("Data Source=Users.db"));
        services.AddScoped<IUnitOfWork>(service => service.GetRequiredService<UserContext>());
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}