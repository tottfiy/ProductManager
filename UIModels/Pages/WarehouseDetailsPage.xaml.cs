
using Services;
using StorageModels.Entities;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace UIModels.Pages
{
    public partial class WarehouseDetailsPage : Page
    {
        private readonly IStorageService _storageService;
        private readonly int _id;

        public WarehouseDetailsPage(int id)
        {
            InitializeComponent();
            _id = id;
            _storageService = App.ServiceProvider.GetService<IStorageService>();
            LoadData();
        }

        private void LoadData()
        {
            var warehouse = _storageService.GetWarehouseById(_id);
            NameText.Text = warehouse.Name;
            ProductsList.ItemsSource = warehouse.Products;
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = (ProductEntity)ProductsList.SelectedItem;
            if (product != null)
            {
                NavigationService.Navigate(new ProductDetailsPage(product.Id));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
