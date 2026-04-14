using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Windows.Controls;
using ViewModels;

namespace UIModels.Pages
{
    // перша сторінка - домашня
    public partial class HomePage : Page
    {
        private readonly IStorageService _storageService;

        public HomePage()
        {
            InitializeComponent();
            _storageService = App.ServiceProvider.GetRequiredService<IStorageService>();

            LoadData();
        }
        private void LoadData()
        {
            WarehousesList.ItemsSource = _storageService.GetWarehouses();
        }

        private void WarehousesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WarehousesList.SelectedItem is WarehouseViewModel selected)
            {
                NavigationService.Navigate(new WarehouseDetailsPage(selected.Id));
            }
        }
    }
}
