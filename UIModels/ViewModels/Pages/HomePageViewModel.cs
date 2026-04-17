using System.Collections.ObjectModel;
using Services;
using Services.DTOs;
using UIModels.Navigation;

namespace UIModels.ViewModels.Pages;

public class HomePageViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private WarehouseListDto? _selectedWarehouse;

    public HomePageViewModel(IStorageService storageService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        Warehouses = new ObservableCollection<WarehouseListDto>(storageService.GetWarehouseList());
    }

    public ObservableCollection<WarehouseListDto> Warehouses { get; }

    public WarehouseListDto? SelectedWarehouse
    {
        get => _selectedWarehouse;
        set
        {
            if (!SetProperty(ref _selectedWarehouse, value))
            {
                return;
            }

            if (value is null)
            {
                return;
            }

            _navigationService.ShowWarehouseDetails(value.Id);
            _selectedWarehouse = null;
            OnPropertyChanged();
        }
    }
}
