
using Services;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace UIModels.Pages
{
    public partial class ProductDetailsPage : Page
    {
        private readonly IStorageService _storageService;
        private readonly int _id;

        public ProductDetailsPage(int id)
        {
            InitializeComponent();
            _id = id;
            _storageService = App.ServiceProvider.GetService<IStorageService>();
            LoadData();
        }

        private void LoadData()
        {
            var product = _storageService.GetProductById(_id);
            NameText.Text = $"Name: {product.Name}";
            QuantityText.Text = $"Quantity: {product.Quantity}";

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
