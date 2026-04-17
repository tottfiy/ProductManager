using Microsoft.Extensions.DependencyInjection;
using Repositories;

namespace Services.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IStorageService, StorageService>();

        return services;
    }
}
