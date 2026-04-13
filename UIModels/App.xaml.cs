using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Windows;

namespace UIModels
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IStorageService, StorageService>();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}