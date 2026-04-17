using Services;
using UIModels.Commands;
using UIModels.Navigation;

namespace UIModels.ViewModels.Pages;

public class ProductDetailsPageViewModel : ViewModelBase
{
    public ProductDetailsPageViewModel(IStorageService storageService, INavigationService navigationService, int productId)
    {
        BackCommand = new RelayCommand(navigationService.GoBack, () => navigationService.CanGoBack);
        navigationService.NavigationChanged += () => BackCommand.RaiseCanExecuteChanged();

        var product = storageService.GetProductDetails(productId);
        if (product is null)
        {
            Id = 0;
            Name = "Товар не знайдено";
            CategoryName = "—";
            Quantity = 0;
            Price = 0m;
            TotalValue = 0m;
            Description = "—";
            WarehouseName = "—";
            return;
        }

        Id = product.Id;
        Name = product.Name;
        CategoryName = product.CategoryName;
        Quantity = product.Quantity;
        Price = product.Price;
        TotalValue = product.TotalValue;
        Description = product.Description;
        WarehouseName = product.WarehouseName;
    }

    public int Id { get; }
    public string Name { get; }
    public string CategoryName { get; }
    public int Quantity { get; }
    public decimal Price { get; }
    public decimal TotalValue { get; }
    public string? Description { get; }
    public string WarehouseName { get; }
    public RelayCommand BackCommand { get; }
}
