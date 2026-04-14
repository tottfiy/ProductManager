using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UIModels.Pages
{
    // друга сторінка з трьох - для виводу конкретного продукту
    public partial class ProductDetailsPage : Page
    {
        private readonly IStorageService _storageService;
        private readonly int _productId;

        public ProductDetailsPage(int productId)
        {
            InitializeComponent();
            _productId = productId;
            _storageService = App.ServiceProvider.GetRequiredService<IStorageService>();
            LoadData();
        }
        private void LoadData()
        {
            // як і в складу фікс повернення типів
            ProductViewModel? product = _storageService.GetProductById(_productId);

            if (product == null)
                return;

            NameText.Text = $"Product: {product.Name}";
            CategoryText.Text = $"Category: {product.ProductCategory}";
            QuantityText.Text = $"Quantity: {product.Quantity} pcs";
            PriceText.Text = $"Price: {product.Price:C2}";
            TotalValueText.Text = $"Total value: {product.TotalValue:C2}";
            DescriptionText.Text = $"Description: {product.Description ?? "—"}";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
