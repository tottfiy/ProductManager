using Microsoft.Extensions.DependencyInjection;
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
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddTransient<HomePageViewModel>();
        services.AddTransient<Func<int, WarehouseDetailsPageViewModel>>(serviceProvider =>
            warehouseId => ActivatorUtilities.CreateInstance<WarehouseDetailsPageViewModel>(serviceProvider, warehouseId));
        services.AddTransient<Func<int, ProductDetailsPageViewModel>>(serviceProvider =>
            productId => ActivatorUtilities.CreateInstance<ProductDetailsPageViewModel>(serviceProvider, productId));

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }
}
