using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UIModels.Infrastructure;

namespace UIModels;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ServiceProvider serviceProvider = AppHost.BuildServiceProvider();
        MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
