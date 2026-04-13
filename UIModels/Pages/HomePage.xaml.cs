
using Services;
using StorageModels.Entities;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace UIModels.Pages
{
    public partial class HomePage : Page
    {
        private readonly IStorageService _storageService;

        public HomePage()
        {
            InitializeComponent();
            _storageService = App.ServiceProvider.GetService<IStorageService>();
            LoadData();
        }

        private void LoadData()
        {
            WarehousesList.ItemsSource = _storageService.GetWarehouses();
        }

        private void WarehousesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (WarehouseEntity)WarehousesList.SelectedItem;
            if (selected != null)
            {
                NavigationService.Navigate(new WarehouseDetailsPage(selected.Id));
            }
        }
    }
}
