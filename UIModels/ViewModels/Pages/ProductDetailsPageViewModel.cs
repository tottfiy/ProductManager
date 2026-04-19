using System.Globalization;
using Services;
using Services.DTOs;
using StorageModels.Enums;
using UIModels.Commands;
using UIModels.Navigation;

namespace UIModels.ViewModels.Pages;

public class ProductDetailsPageViewModel : PageViewModelBase
{
    private readonly IStorageService _storageService;
    private readonly INavigationService _navigationService;
    private readonly int? _productId;
    private readonly int? _warehouseId;
    private int _id;
    private int _warehouseEntityId;
    private string _warehouseName = string.Empty;
    private string _name = string.Empty;
    private string _quantityText = "0";
    private string _priceText = "0";
    private string _description = string.Empty;
    private ProductCategory _selectedCategory = ProductCategory.Electronics;
    private bool _isEditMode;

    public ProductDetailsPageViewModel(IStorageService storageService, INavigationService navigationService, int? productId, int? warehouseId)
    {
        _storageService = storageService;
        _navigationService = navigationService;
        _productId = productId;
        _warehouseId = warehouseId;
        _isEditMode = productId is null;

        BackCommand = new RelayCommand(_navigationService.GoBack, () => !IsBusy && _navigationService.CanGoBack);
        EditCommand = new RelayCommand(() => IsEditMode = true, () => !IsBusy && !IsCreateMode && !IsEditMode);
        CancelEditCommand = new AsyncRelayCommand(CancelEditAsync, () => !IsBusy && IsEditMode);
        SaveCommand = new AsyncRelayCommand(SaveAsync, () => !IsBusy && IsEditMode);
        DeleteProductCommand = new AsyncRelayCommand(DeleteProductAsync, () => !IsBusy && !IsCreateMode);
        RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);

        _navigationService.NavigationChanged += HandleNavigationChanged;
    }

    public bool IsCreateMode => !_productId.HasValue;

    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            if (SetProperty(ref _isEditMode, value))
            {
                OnPropertyChanged(nameof(IsReadOnlyMode));
                RaiseCommandStates();
            }
        }
    }

    public bool IsReadOnlyMode => !IsEditMode;

    public string Title => IsCreateMode ? "Створення товару" : $"Товар №{Id}";

    public string DisplayId => IsCreateMode ? "—" : Id.ToString();

    public int Id
    {
        get => _id;
        private set
        {
            if (SetProperty(ref _id, value))
            {
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayId));
            }
        }
    }

    public int WarehouseEntityId
    {
        get => _warehouseEntityId;
        private set => SetProperty(ref _warehouseEntityId, value);
    }

    public string WarehouseName
    {
        get => _warehouseName;
        private set => SetProperty(ref _warehouseName, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string QuantityText
    {
        get => _quantityText;
        set
        {
            if (SetProperty(ref _quantityText, value))
            {
                OnPropertyChanged(nameof(TotalValuePreview));
            }
        }
    }

    public string PriceText
    {
        get => _priceText;
        set
        {
            if (SetProperty(ref _priceText, value))
            {
                OnPropertyChanged(nameof(TotalValuePreview));
            }
        }
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public ProductCategory SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public IEnumerable<ProductCategory> Categories => Enum.GetValues<ProductCategory>();

    public decimal TotalValuePreview
    {
        get
        {
            bool quantityParsed = TryParseQuantity(out int quantity);
            bool priceParsed = TryParsePrice(out decimal price);
            if (!quantityParsed || !priceParsed)
            {
                return 0m;
            }

            return quantity * price;
        }
    }

    public RelayCommand BackCommand { get; }

    public RelayCommand EditCommand { get; }

    public AsyncRelayCommand CancelEditCommand { get; }

    public AsyncRelayCommand SaveCommand { get; }

    public AsyncRelayCommand DeleteProductCommand { get; }

    public AsyncRelayCommand RefreshCommand { get; }

    public override async Task LoadAsync()
    {
        if (IsCreateMode)
        {
            await ExecuteBusyActionAsync("Підготовка форми товару...", async () =>
            {
                if (!_warehouseId.HasValue)
                {
                    ShowError("Не вибрано склад для нового товару.");
                    _navigationService.ShowHome();
                    return;
                }

                string? warehouseName = await _storageService.GetWarehouseNameAsync(_warehouseId.Value);
                if (string.IsNullOrWhiteSpace(warehouseName))
                {
                    ShowError("Склад для нового товару не знайдено.");
                    _navigationService.ShowHome();
                    return;
                }

                Id = 0;
                WarehouseEntityId = _warehouseId.Value;
                WarehouseName = warehouseName;
                Name = string.Empty;
                QuantityText = "0";
                PriceText = "0";
                Description = string.Empty;
                SelectedCategory = ProductCategory.Electronics;
            });

            return;
        }

        await ExecuteBusyActionAsync("Завантаження товару...", LoadProductDataAsync);
    }

    private async Task LoadProductDataAsync()
    {
        ProductDetailsDto? product = await _storageService.GetProductDetailsAsync(_productId!.Value);
        if (product is null)
        {
            ShowError("Товар не знайдено.");
            _navigationService.GoBack();
            return;
        }

        ApplyProductDetails(product);
    }

    private void ApplyProductDetails(ProductDetailsDto product)
    {
        Id = product.Id;
        WarehouseEntityId = product.WarehouseId;
        WarehouseName = product.WarehouseName;
        Name = product.Name;
        QuantityText = product.Quantity.ToString(CultureInfo.CurrentCulture);
        PriceText = product.Price.ToString(CultureInfo.CurrentCulture);
        Description = product.Description == "—" ? string.Empty : product.Description;
        SelectedCategory = product.Category;
        OnPropertyChanged(nameof(TotalValuePreview));
    }

    private async Task SaveAsync()
    {
        try
        {
            if (!TryParseQuantity(out int quantity))
            {
                ShowError("Введіть коректну кількість товару.");
                return;
            }

            if (!TryParsePrice(out decimal price))
            {
                ShowError("Введіть коректну ціну товару.");
                return;
            }

            await ExecuteBusyActionAsync("Збереження товару...", async () =>
            {
                ProductDetailsDto? product = await _storageService.SaveProductAsync(new ProductSaveDto
                {
                    Id = IsCreateMode ? null : Id,
                    WarehouseId = WarehouseEntityId,
                    Name = Name,
                    Quantity = quantity,
                    Price = price,
                    Category = SelectedCategory,
                    Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim()
                });

                if (product is null)
                {
                    ShowError("Не вдалося зберегти товар.");
                    return;
                }

                if (IsCreateMode)
                {
                    _navigationService.ShowProductDetails(product.Id);
                    return;
                }

                IsEditMode = false;
                ApplyProductDetails(product);
            });
        }
        catch (Exception exception)
        {
            ShowError(exception.Message);
        }
    }

    private async Task CancelEditAsync()
    {
        if (IsCreateMode)
        {
            _navigationService.GoBack();
            return;
        }

        IsEditMode = false;
        await LoadAsync();
    }

    private async Task DeleteProductAsync()
    {
        if (IsCreateMode)
        {
            return;
        }

        bool shouldDelete = System.Windows.MessageBox.Show(
            $"Видалити товар \"{Name}\"?",
            "Підтвердження видалення",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes;

        if (!shouldDelete)
        {
            return;
        }

        await ExecuteBusyActionAsync("Видалення товару...", async () =>
        {
            bool deleted = await _storageService.DeleteProductAsync(Id);
            if (!deleted)
            {
                ShowError("Не вдалося видалити товар.");
                return;
            }

            _navigationService.ShowWarehouseDetails(WarehouseEntityId);
        });
    }

    private bool TryParseQuantity(out int quantity)
    {
        return int.TryParse(QuantityText, NumberStyles.Integer, CultureInfo.CurrentCulture, out quantity) && quantity >= 0;
    }

    private bool TryParsePrice(out decimal price)
    {
        if (decimal.TryParse(PriceText, NumberStyles.Number, CultureInfo.CurrentCulture, out price))
        {
            return price >= 0;
        }

        if (decimal.TryParse(PriceText, NumberStyles.Number, CultureInfo.InvariantCulture, out price))
        {
            return price >= 0;
        }

        return false;
    }

    private void HandleNavigationChanged()
    {
        RaiseCommandStates();
    }

    protected override void RaiseCommandStates()
    {
        BackCommand.RaiseCanExecuteChanged();
        EditCommand.RaiseCanExecuteChanged();
        CancelEditCommand.RaiseCanExecuteChanged();
        SaveCommand.RaiseCanExecuteChanged();
        DeleteProductCommand.RaiseCanExecuteChanged();
        RefreshCommand.RaiseCanExecuteChanged();
    }
}
