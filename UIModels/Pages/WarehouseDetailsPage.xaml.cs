using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UIModels.Pages
{

    // сторінка для виволу всіх складів

    public partial class WarehouseDetailsPage : Page
    {
        private readonly IStorageService _storageService;
        private readonly int _warehouseId;

        public WarehouseDetailsPage(int warehouseId)
        {
            InitializeComponent();
            _warehouseId = warehouseId;


            _storageService = App.ServiceProvider.GetRequiredService<IStorageService>();

            LoadData();
        }


        private void LoadData()
        {
            // з минулого апдейту змінив повернення GetWarehouseById на правильний тип WarehouseViewModel
            WarehouseViewModel? warehouse = _storageService.GetWarehouseById(_warehouseId);

            if (warehouse == null)
                return;

            NameText.Text = $"Warehouse: {warehouse.Name}";
            LocationText.Text = $"Location: {warehouse.Location}";
            TotalValueText.Text = $"Total inventory value: {warehouse.TotalValue:C2}";

            ProductsList.ItemsSource = warehouse.Products;
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsList.SelectedItem is ProductViewModel product)
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
