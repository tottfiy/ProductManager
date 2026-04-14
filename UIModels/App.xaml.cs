using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Windows;

namespace UIModels
{
    // Використання DI та IoC в роботі за сховищем
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // пофікшено з минулого апдейту
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddSingleton<IStorageService, StorageService>();
            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
