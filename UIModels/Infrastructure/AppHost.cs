using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.DependencyInjection;
using UIModels.Navigation;
using UIModels.ViewModels;
using UIModels.ViewModels.Pages;

namespace UIModels.Infrastructure;

public static class AppHost
{
    public static ServiceProvider BuildServiceProvider()
    {
        ServiceCollection services = new();

        services.AddStorageServices();

        services.AddSingleton<NavigationStore>();

        services.AddTransient<HomePageViewModel>();
        services.AddSingleton<Func<HomePageViewModel>>(serviceProvider =>
            () => new HomePageViewModel(
                serviceProvider.GetRequiredService<IStorageService>(),
                serviceProvider.GetRequiredService<INavigationService>()));
        services.AddSingleton<Func<int?, WarehouseDetailsPageViewModel>>(serviceProvider =>
            warehouseId => new WarehouseDetailsPageViewModel(
                serviceProvider.GetRequiredService<IStorageService>(),
                serviceProvider.GetRequiredService<INavigationService>(),
                warehouseId));
        services.AddSingleton<Func<int?, int?, ProductDetailsPageViewModel>>(serviceProvider =>
            (productId, warehouseId) => new ProductDetailsPageViewModel(
                serviceProvider.GetRequiredService<IStorageService>(),
                serviceProvider.GetRequiredService<INavigationService>(),
                productId,
                warehouseId));

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });
    }
}
