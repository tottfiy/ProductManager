using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services;
using System.Windows;
using UIModels.Navigation;
using UIModels.ViewModels;
using UIModels.ViewModels.Pages;

namespace UIModels;

public partial class App : Application
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IStorageService, StorageService>();

        services.AddSingleton<NavigationStore>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddTransient<HomePageViewModel>();
        services.AddTransient<Func<int, WarehouseDetailsPageViewModel>>(serviceProvider =>
            warehouseId => ActivatorUtilities.CreateInstance<WarehouseDetailsPageViewModel>(serviceProvider, warehouseId));
        services.AddTransient<Func<int, ProductDetailsPageViewModel>>(serviceProvider =>
            productId => ActivatorUtilities.CreateInstance<ProductDetailsPageViewModel>(serviceProvider, productId));

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
    }
}
