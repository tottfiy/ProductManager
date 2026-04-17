using System.Collections.ObjectModel;
using Services;
using Services.DTOs;
using UIModels.Commands;
using UIModels.Navigation;

namespace UIModels.ViewModels.Pages;

public class WarehouseDetailsPageViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private ProductListDto? _selectedProduct;

    public WarehouseDetailsPageViewModel(IStorageService storageService, INavigationService navigationService, int warehouseId)
    {
        _navigationService = navigationService;
        BackCommand = new RelayCommand(_navigationService.GoBack, () => _navigationService.CanGoBack);
        _navigationService.NavigationChanged += () => BackCommand.RaiseCanExecuteChanged();

        WarehouseDetailsDto? warehouse = storageService.GetWarehouseDetails(warehouseId);
        if (warehouse is null)
        {
            Id = 0;
            Name = "Склад не знайдено";
            LocationName = "—";
            TotalValue = 0m;
            Products = new ObservableCollection<ProductListDto>();
            return;
        }

        Id = warehouse.Id;
        Name = warehouse.Name;
        LocationName = warehouse.LocationName;
        TotalValue = warehouse.TotalValue;
        Products = new ObservableCollection<ProductListDto>(warehouse.Products);
    }

    public int Id { get; }
    public string Name { get; }
    public string LocationName { get; }
    public decimal TotalValue { get; }
    public ObservableCollection<ProductListDto> Products { get; }
    public RelayCommand BackCommand { get; }

    public ProductListDto? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (!SetProperty(ref _selectedProduct, value))
            {
                return;
            }

            if (value is null)
            {
                return;
            }

            _navigationService.ShowProductDetails(value.Id);
            _selectedProduct = null;
            OnPropertyChanged();
        }
    }
}
