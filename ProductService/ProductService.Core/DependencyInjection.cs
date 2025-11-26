using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.Services;
using ProductService.Infrastructure;

namespace ProductService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddScoped<IProductService, Services.ProductService>();
        return services;
    }

    public static void ApplyMigrations(IServiceProvider serviceProvider)
    {
        Infrastructure.DependencyInjection.MigrateDatabase(serviceProvider);
    }
}
