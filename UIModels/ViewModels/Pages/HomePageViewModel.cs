using System.Collections.ObjectModel;
using Services;
using Services.DTOs;
using UIModels.Commands;
using UIModels.Navigation;

namespace UIModels.ViewModels.Pages;

public class HomePageViewModel : PageViewModelBase
{
    private const string SortByNameAscending = "Назва (А-Я)";
    private const string SortByNameDescending = "Назва (Я-А)";
    private const string SortByLocation = "Локація";
    private const string SortByProductCount = "Кількість товарів";
    private const string SortByTotalValue = "Загальна вартість";

    private readonly IStorageService _storageService;
    private readonly INavigationService _navigationService;
    private List<WarehouseListDto> _allWarehouses = new();
    private WarehouseListDto? _selectedWarehouse;
    private string _searchText = string.Empty;
    private string _selectedSortOption = SortByNameAscending;

    public HomePageViewModel(IStorageService storageService, INavigationService navigationService)
    {
        _storageService = storageService;
        _navigationService = navigationService;

        SortOptions =
        [
            SortByNameAscending,
            SortByNameDescending,
            SortByLocation,
            SortByProductCount,
            SortByTotalValue
        ];

        Warehouses = new ObservableCollection<WarehouseListDto>();

        RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
        DeleteWarehouseCommand = new AsyncRelayCommand(DeleteSelectedWarehouseAsync, () => !IsBusy && SelectedWarehouse is not null);
        OpenWarehouseCommand = new RelayCommand(OpenSelectedWarehouse, () => !IsBusy && SelectedWarehouse is not null);
        AddWarehouseCommand = new RelayCommand(AddWarehouse, () => !IsBusy);
    }

    public ObservableCollection<WarehouseListDto> Warehouses { get; }

    public IReadOnlyList<string> SortOptions { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                ApplyFilters();
            }
        }
    }

    public string SelectedSortOption
    {
        get => _selectedSortOption;
        set
        {
            if (SetProperty(ref _selectedSortOption, value))
            {
                ApplyFilters();
            }
        }
    }

    public WarehouseListDto? SelectedWarehouse
    {
        get => _selectedWarehouse;
        set
        {
            if (SetProperty(ref _selectedWarehouse, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public AsyncRelayCommand RefreshCommand { get; }

    public AsyncRelayCommand DeleteWarehouseCommand { get; }

    public RelayCommand OpenWarehouseCommand { get; }

    public RelayCommand AddWarehouseCommand { get; }

    public override async Task LoadAsync()
    {
        await ExecuteBusyActionAsync("Завантаження складів...", async () =>
        {
            IReadOnlyCollection<WarehouseListDto> warehouses = await _storageService.GetWarehouseListAsync();
            _allWarehouses = warehouses.ToList();
            ApplyFilters();
        });
    }

    private void ApplyFilters()
    {
        IEnumerable<WarehouseListDto> query = _allWarehouses;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            string search = SearchText.Trim();
            query = query.Where(warehouse =>
                warehouse.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                warehouse.LocationName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        query = SelectedSortOption switch
        {
            SortByNameDescending => query.OrderByDescending(warehouse => warehouse.Name),
            SortByLocation => query.OrderBy(warehouse => warehouse.LocationName).ThenBy(warehouse => warehouse.Name),
            SortByProductCount => query.OrderByDescending(warehouse => warehouse.ProductCount).ThenBy(warehouse => warehouse.Name),
            SortByTotalValue => query.OrderByDescending(warehouse => warehouse.TotalValue).ThenBy(warehouse => warehouse.Name),
            _ => query.OrderBy(warehouse => warehouse.Name)
        };

        ReplaceCollection(Warehouses, query);
    }

    private async Task DeleteSelectedWarehouseAsync()
    {
        if (SelectedWarehouse is null)
        {
            return;
        }

        WarehouseListDto warehouseToDelete = SelectedWarehouse;
        bool shouldDelete = System.Windows.MessageBox.Show(
            $"Видалити склад \"{warehouseToDelete.Name}\" разом з усіма його товарами?",
            "Підтвердження видалення",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes;

        if (!shouldDelete)
        {
            return;
        }

        await ExecuteBusyActionAsync("Видалення складу...", async () =>
        {
            bool deleted = await _storageService.DeleteWarehouseAsync(warehouseToDelete.Id);
            if (!deleted)
            {
                ShowError("Не вдалося видалити склад.");
                return;
            }

            SelectedWarehouse = null;
            IReadOnlyCollection<WarehouseListDto> warehouses = await _storageService.GetWarehouseListAsync();
            _allWarehouses = warehouses.ToList();
            ApplyFilters();
        });
    }

    private void OpenSelectedWarehouse()
    {
        if (SelectedWarehouse is null)
        {
            return;
        }

        _navigationService.ShowWarehouseDetails(SelectedWarehouse.Id);
    }

    private void AddWarehouse()
    {
        _navigationService.ShowWarehouseDetails();
    }

    protected override void RaiseCommandStates()
    {
        RefreshCommand.RaiseCanExecuteChanged();
        DeleteWarehouseCommand.RaiseCanExecuteChanged();
        OpenWarehouseCommand.RaiseCanExecuteChanged();
        AddWarehouseCommand.RaiseCanExecuteChanged();
    }
}
