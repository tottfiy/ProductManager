using System.Collections.ObjectModel;
using System.Globalization;
using Services;
using Services.DTOs;
using StorageModels.Enums;
using UIModels.Commands;
using UIModels.Navigation;

namespace UIModels.ViewModels.Pages;

public class WarehouseDetailsPageViewModel : PageViewModelBase
{
    private const string SortByNameAscending = "Назва (А-Я)";
    private const string SortByNameDescending = "Назва (Я-А)";
    private const string SortByCategory = "Категорія";
    private const string SortByQuantity = "Кількість";
    private const string SortByPrice = "Ціна";
    private const string SortByTotalValue = "Вартість";

    private readonly IStorageService _storageService;
    private readonly INavigationService _navigationService;
    private readonly int? _warehouseId;
    private List<ProductListDto> _allProducts = new();
    private int _id;
    private string _name = string.Empty;
    private Location _selectedLocation = Location.Kyiv;
    private decimal _totalValue;
    private bool _isEditMode;
    private AddProductMode _addProductMode = AddProductMode.None;
    private ProductListDto? _selectedProduct;
    private string _productSearchText = string.Empty;
    private string _selectedProductSortOption = SortByNameAscending;
    private string _productSectionHint = string.Empty;
    private string _newProductName = string.Empty;
    private string _newProductQuantityText = "0";
    private string _newProductPriceText = "0";
    private string _newProductDescription = string.Empty;
    private ProductCategory _newSelectedProductCategory = ProductCategory.Electronics;
    private ExistingProductOptionDto? _selectedExistingProduct;
    private string _existingProductQuantityText = "1";

    public WarehouseDetailsPageViewModel(IStorageService storageService, INavigationService navigationService, int? warehouseId)
    {
        _storageService = storageService;
        _navigationService = navigationService;
        _warehouseId = warehouseId;
        _isEditMode = warehouseId is null;

        ProductSortOptions =
        [
            SortByNameAscending,
            SortByNameDescending,
            SortByCategory,
            SortByQuantity,
            SortByPrice,
            SortByTotalValue
        ];

        Products = new ObservableCollection<ProductListDto>();
        ExistingProducts = new ObservableCollection<ExistingProductOptionDto>();

        BackCommand = new RelayCommand(_navigationService.GoBack, () => !IsBusy && _navigationService.CanGoBack);
        EditCommand = new RelayCommand(() => IsEditMode = true, () => !IsBusy && !IsCreateMode && !IsEditMode && !IsAnyAddProductMode);
        CancelEditCommand = new AsyncRelayCommand(CancelEditAsync, () => !IsBusy && IsEditMode);
        SaveCommand = new AsyncRelayCommand(SaveAsync, () => !IsBusy && IsEditMode);
        DeleteWarehouseCommand = new AsyncRelayCommand(DeleteWarehouseAsync, () => !IsBusy && !IsCreateMode && !IsAnyAddProductMode);
        RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy && !IsAnyAddProductMode);
        AddNewProductCommand = new RelayCommand(BeginAddNewProduct, () => CanManageProducts);
        AddExistingProductCommand = new AsyncRelayCommand(BeginAddExistingProductAsync, () => CanManageProducts);
        SaveNewProductCommand = new AsyncRelayCommand(SaveNewProductAsync, () => !IsBusy && IsAddNewProductMode);
        SaveExistingProductCommand = new AsyncRelayCommand(SaveExistingProductAsync, () => !IsBusy && IsAddExistingProductMode && SelectedExistingProduct is not null);
        CancelAddProductCommand = new RelayCommand(CancelAddProduct, () => !IsBusy && IsAnyAddProductMode);
        OpenProductCommand = new RelayCommand(OpenSelectedProduct, () => !IsBusy && SelectedProduct is not null && !IsEditMode && !IsAnyAddProductMode);
        DeleteSelectedProductCommand = new AsyncRelayCommand(DeleteSelectedProductAsync, () => !IsBusy && SelectedProduct is not null && !IsEditMode && !IsAnyAddProductMode);

        _navigationService.NavigationChanged += HandleNavigationChanged;
        UpdateProductSectionHint();
    }

    public bool IsCreateMode => !_warehouseId.HasValue;

    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            if (SetProperty(ref _isEditMode, value))
            {
                if (value)
                {
                    SetAddProductMode(AddProductMode.None);
                }

                OnPropertyChanged(nameof(IsReadOnlyMode));
                OnPropertyChanged(nameof(CanManageProducts));
                UpdateProductSectionHint();
                RaiseCommandStates();
            }
        }
    }

    public bool IsAddNewProductMode => _addProductMode == AddProductMode.New;

    public bool IsAddExistingProductMode => _addProductMode == AddProductMode.Existing;

    public bool IsAnyAddProductMode => _addProductMode != AddProductMode.None;

    public bool IsReadOnlyMode => !IsEditMode;

    public bool CanManageProducts => !IsBusy && !IsCreateMode && !IsEditMode && !IsAnyAddProductMode;

    public string Title => IsCreateMode ? "Створення складу" : $"Склад №{Id}";

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

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public Location SelectedLocation
    {
        get => _selectedLocation;
        set => SetProperty(ref _selectedLocation, value);
    }

    public decimal TotalValue
    {
        get => _totalValue;
        private set => SetProperty(ref _totalValue, value);
    }

    public IEnumerable<Location> Locations => Enum.GetValues<Location>();

    public ObservableCollection<ProductListDto> Products { get; }

    public ObservableCollection<ExistingProductOptionDto> ExistingProducts { get; }

    public IReadOnlyList<string> ProductSortOptions { get; }

    public IEnumerable<ProductCategory> ProductCategories => Enum.GetValues<ProductCategory>();

    public string ProductSearchText
    {
        get => _productSearchText;
        set
        {
            if (SetProperty(ref _productSearchText, value))
            {
                ApplyProductFilters();
            }
        }
    }

    public string SelectedProductSortOption
    {
        get => _selectedProductSortOption;
        set
        {
            if (SetProperty(ref _selectedProductSortOption, value))
            {
                ApplyProductFilters();
            }
        }
    }

    public string ProductSectionHint
    {
        get => _productSectionHint;
        private set => SetProperty(ref _productSectionHint, value);
    }

    public ProductListDto? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (SetProperty(ref _selectedProduct, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public string NewProductName
    {
        get => _newProductName;
        set => SetProperty(ref _newProductName, value);
    }

    public string NewProductQuantityText
    {
        get => _newProductQuantityText;
        set => SetProperty(ref _newProductQuantityText, value);
    }

    public string NewProductPriceText
    {
        get => _newProductPriceText;
        set => SetProperty(ref _newProductPriceText, value);
    }

    public string NewProductDescription
    {
        get => _newProductDescription;
        set => SetProperty(ref _newProductDescription, value);
    }

    public ProductCategory NewSelectedProductCategory
    {
        get => _newSelectedProductCategory;
        set => SetProperty(ref _newSelectedProductCategory, value);
    }

    public ExistingProductOptionDto? SelectedExistingProduct
    {
        get => _selectedExistingProduct;
        set
        {
            if (SetProperty(ref _selectedExistingProduct, value))
            {
                OnPropertyChanged(nameof(SelectedExistingProductName));
                OnPropertyChanged(nameof(SelectedExistingProductCategoryName));
                OnPropertyChanged(nameof(SelectedExistingProductPriceDisplay));
                OnPropertyChanged(nameof(SelectedExistingProductSourceWarehouseName));
                OnPropertyChanged(nameof(SelectedExistingProductSourceQuantityDisplay));
                OnPropertyChanged(nameof(SelectedExistingProductDescription));
                RaiseCommandStates();
            }
        }
    }

    public string ExistingProductQuantityText
    {
        get => _existingProductQuantityText;
        set => SetProperty(ref _existingProductQuantityText, value);
    }

    public string SelectedExistingProductName => SelectedExistingProduct?.Name ?? "—";

    public string SelectedExistingProductCategoryName => SelectedExistingProduct?.CategoryName ?? "—";

    public string SelectedExistingProductPriceDisplay => SelectedExistingProduct is null ? "—" : SelectedExistingProduct.Price.ToString("C2");

    public string SelectedExistingProductSourceWarehouseName => SelectedExistingProduct?.SourceWarehouseName ?? "—";

    public string SelectedExistingProductSourceQuantityDisplay => SelectedExistingProduct is null ? "—" : SelectedExistingProduct.SourceQuantity.ToString();

    public string SelectedExistingProductDescription => SelectedExistingProduct?.Description ?? "—";

    public RelayCommand BackCommand { get; }

    public RelayCommand EditCommand { get; }

    public AsyncRelayCommand CancelEditCommand { get; }

    public AsyncRelayCommand SaveCommand { get; }

    public AsyncRelayCommand DeleteWarehouseCommand { get; }

    public AsyncRelayCommand RefreshCommand { get; }

    public RelayCommand AddNewProductCommand { get; }

    public AsyncRelayCommand AddExistingProductCommand { get; }

    public AsyncRelayCommand SaveNewProductCommand { get; }

    public AsyncRelayCommand SaveExistingProductCommand { get; }

    public RelayCommand CancelAddProductCommand { get; }

    public RelayCommand OpenProductCommand { get; }

    public AsyncRelayCommand DeleteSelectedProductCommand { get; }

    public override async Task LoadAsync()
    {
        if (IsCreateMode)
        {
            Id = 0;
            Name = string.Empty;
            SelectedLocation = Location.Kyiv;
            TotalValue = 0m;
            _allProducts = new List<ProductListDto>();
            SelectedProduct = null;
            ResetAddProductModes();
            ApplyProductFilters();
            UpdateProductSectionHint();
            RaiseCommandStates();
            return;
        }

        await ExecuteBusyActionAsync("Завантаження складу...", LoadWarehouseDataAsync);
    }

    private async Task LoadWarehouseDataAsync()
    {
        WarehouseDetailsDto? warehouse = await _storageService.GetWarehouseDetailsAsync(_warehouseId!.Value);
        if (warehouse is null)
        {
            ShowError("Склад не знайдено.");
            _navigationService.ShowHome();
            return;
        }

        ApplyWarehouseDetails(warehouse);
    }

    private void ApplyWarehouseDetails(WarehouseDetailsDto warehouse)
    {
        Id = warehouse.Id;
        Name = warehouse.Name;
        SelectedLocation = warehouse.Location;
        TotalValue = warehouse.TotalValue;
        _allProducts = warehouse.Products.ToList();
        SelectedProduct = null;
        ApplyProductFilters();
        UpdateProductSectionHint();
    }

    private void ApplyProductFilters()
    {
        IEnumerable<ProductListDto> query = _allProducts;

        if (!string.IsNullOrWhiteSpace(ProductSearchText))
        {
            string search = ProductSearchText.Trim();
            query = query.Where(product =>
                product.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                product.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        query = SelectedProductSortOption switch
        {
            SortByNameDescending => query.OrderByDescending(product => product.Name),
            SortByCategory => query.OrderBy(product => product.CategoryName).ThenBy(product => product.Name),
            SortByQuantity => query.OrderByDescending(product => product.Quantity).ThenBy(product => product.Name),
            SortByPrice => query.OrderByDescending(product => product.Price).ThenBy(product => product.Name),
            SortByTotalValue => query.OrderByDescending(product => product.TotalValue).ThenBy(product => product.Name),
            _ => query.OrderBy(product => product.Name)
        };

        ReplaceCollection(Products, query);
    }

    private async Task SaveAsync()
    {
        try
        {
            await ExecuteBusyActionAsync("Збереження складу...", async () =>
            {
                WarehouseDetailsDto warehouse = await _storageService.SaveWarehouseAsync(new WarehouseSaveDto
                {
                    Id = IsCreateMode ? null : Id,
                    Name = Name,
                    Location = SelectedLocation
                });

                if (IsCreateMode)
                {
                    _navigationService.ShowWarehouseDetails(warehouse.Id);
                    return;
                }

                IsEditMode = false;
                ApplyWarehouseDetails(warehouse);
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

    private async Task DeleteWarehouseAsync()
    {
        if (IsCreateMode)
        {
            return;
        }

        bool shouldDelete = System.Windows.MessageBox.Show(
            $"Видалити склад \"{Name}\" разом з усіма його товарами?",
            "Підтвердження видалення",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes;

        if (!shouldDelete)
        {
            return;
        }

        await ExecuteBusyActionAsync("Видалення складу...", async () =>
        {
            bool deleted = await _storageService.DeleteWarehouseAsync(Id);
            if (!deleted)
            {
                ShowError("Не вдалося видалити склад.");
                return;
            }

            _navigationService.ShowHome();
        });
    }

    private void BeginAddNewProduct()
    {
        if (!CanManageProducts)
        {
            return;
        }

        ResetNewProductForm();
        SelectedProduct = null;
        SetAddProductMode(AddProductMode.New);
    }

    private async Task BeginAddExistingProductAsync()
    {
        if (!CanManageProducts || IsCreateMode)
        {
            return;
        }

        try
        {
            await ExecuteBusyActionAsync("Завантаження доступних товарів...", async () =>
            {
                IReadOnlyCollection<ExistingProductOptionDto> existingProducts = await _storageService.GetExistingProductOptionsAsync(Id);
                ReplaceCollection(ExistingProducts, existingProducts);
                SelectedExistingProduct = ExistingProducts.FirstOrDefault();
                ExistingProductQuantityText = "1";

                if (ExistingProducts.Count == 0)
                {
                    SetAddProductMode(AddProductMode.None);
                    ShowInfo("Немає доступних існуючих товарів для додавання.");
                    return;
                }

                SelectedProduct = null;
                SetAddProductMode(AddProductMode.Existing);
            });
        }
        catch (Exception exception)
        {
            ShowError(exception.Message);
        }
    }

    private async Task SaveNewProductAsync()
    {
        if (IsCreateMode)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(NewProductName))
        {
            ShowError("Назва товару не може бути порожньою.");
            return;
        }

        if (!TryParseNewProductQuantity(out int quantity))
        {
            ShowError("Введіть коректну кількість товару.");
            return;
        }

        if (!TryParseNewProductPrice(out decimal price))
        {
            ShowError("Введіть коректну ціну товару.");
            return;
        }

        try
        {
            await ExecuteBusyActionAsync("Додавання нового товару...", async () =>
            {
                ProductDetailsDto? product = await _storageService.SaveProductAsync(new ProductSaveDto
                {
                    WarehouseId = Id,
                    Name = NewProductName.Trim(),
                    Quantity = quantity,
                    Price = price,
                    Category = NewSelectedProductCategory,
                    Description = string.IsNullOrWhiteSpace(NewProductDescription) ? null : NewProductDescription.Trim()
                });

                if (product is null)
                {
                    ShowError("Не вдалося додати товар.");
                    return;
                }

                await ReloadWarehouseAfterProductChangeAsync(product.Id, "Товар успішно додано до складу.");
            });
        }
        catch (Exception exception)
        {
            ShowError(exception.Message);
        }
    }

    private async Task SaveExistingProductAsync()
    {
        if (IsCreateMode)
        {
            return;
        }

        if (SelectedExistingProduct is null)
        {
            ShowError("Оберіть існуючий товар зі списку.");
            return;
        }

        if (!TryParseExistingProductQuantity(out int quantity))
        {
            ShowError("Введіть коректну кількість для існуючого товару.");
            return;
        }

        try
        {
            await ExecuteBusyActionAsync("Додавання існуючого товару...", async () =>
            {
                ProductDetailsDto? product = await _storageService.AddExistingProductToWarehouseAsync(Id, SelectedExistingProduct.Id, quantity);
                if (product is null)
                {
                    ShowError("Не вдалося додати існуючий товар до складу.");
                    return;
                }

                await ReloadWarehouseAfterProductChangeAsync(product.Id, "Існуючий товар успішно додано до складу.");
            });
        }
        catch (Exception exception)
        {
            ShowError(exception.Message);
        }
    }

    private async Task ReloadWarehouseAfterProductChangeAsync(int selectedProductId, string successMessage)
    {
        WarehouseDetailsDto? warehouse = await _storageService.GetWarehouseDetailsAsync(Id);
        if (warehouse is null)
        {
            ShowError("Не вдалося оновити список товарів після збереження.");
            return;
        }

        ApplyWarehouseDetails(warehouse);
        ResetAddProductModes();
        SelectedProduct = Products.FirstOrDefault(item => item.Id == selectedProductId);
        ShowInfo(successMessage);
    }

    private void CancelAddProduct()
    {
        ResetAddProductModes();
    }

    private void OpenSelectedProduct()
    {
        if (SelectedProduct is null)
        {
            return;
        }

        _navigationService.ShowProductDetails(SelectedProduct.Id);
    }

    private async Task DeleteSelectedProductAsync()
    {
        if (SelectedProduct is null)
        {
            return;
        }

        ProductListDto productToDelete = SelectedProduct;
        bool shouldDelete = System.Windows.MessageBox.Show(
            $"Видалити товар \"{productToDelete.Name}\"?",
            "Підтвердження видалення",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes;

        if (!shouldDelete)
        {
            return;
        }

        await ExecuteBusyActionAsync("Видалення товару...", async () =>
        {
            bool deleted = await _storageService.DeleteProductAsync(productToDelete.Id);
            if (!deleted)
            {
                ShowError("Не вдалося видалити товар.");
                return;
            }

            WarehouseDetailsDto? warehouse = await _storageService.GetWarehouseDetailsAsync(Id);
            if (warehouse is null)
            {
                ShowError("Не вдалося оновити список товарів після видалення.");
                return;
            }

            ApplyWarehouseDetails(warehouse);
        });
    }

    private bool TryParseNewProductQuantity(out int quantity)
    {
        return int.TryParse(NewProductQuantityText, NumberStyles.Integer, CultureInfo.CurrentCulture, out quantity) && quantity >= 0;
    }

    private bool TryParseNewProductPrice(out decimal price)
    {
        if (decimal.TryParse(NewProductPriceText, NumberStyles.Number, CultureInfo.CurrentCulture, out price))
        {
            return price >= 0;
        }

        if (decimal.TryParse(NewProductPriceText, NumberStyles.Number, CultureInfo.InvariantCulture, out price))
        {
            return price >= 0;
        }

        return false;
    }

    private bool TryParseExistingProductQuantity(out int quantity)
    {
        return int.TryParse(ExistingProductQuantityText, NumberStyles.Integer, CultureInfo.CurrentCulture, out quantity) && quantity > 0;
    }

    private void ResetNewProductForm()
    {
        NewProductName = string.Empty;
        NewProductQuantityText = "0";
        NewProductPriceText = "0";
        NewProductDescription = string.Empty;
        NewSelectedProductCategory = ProductCategory.Electronics;
    }

    private void ResetExistingProductForm()
    {
        ReplaceCollection(ExistingProducts, Array.Empty<ExistingProductOptionDto>());
        SelectedExistingProduct = null;
        ExistingProductQuantityText = "1";
    }

    private void ResetAddProductModes()
    {
        SetAddProductMode(AddProductMode.None);
        ResetNewProductForm();
        ResetExistingProductForm();
    }

    private void SetAddProductMode(AddProductMode mode)
    {
        if (_addProductMode == mode)
        {
            return;
        }

        _addProductMode = mode;
        OnPropertyChanged(nameof(IsAddNewProductMode));
        OnPropertyChanged(nameof(IsAddExistingProductMode));
        OnPropertyChanged(nameof(IsAnyAddProductMode));
        OnPropertyChanged(nameof(CanManageProducts));
        UpdateProductSectionHint();
        RaiseCommandStates();
    }

    private void UpdateProductSectionHint()
    {
        if (IsCreateMode)
        {
            ProductSectionHint = "Спочатку збережіть склад, щоб додавати товари.";
            return;
        }

        if (IsEditMode)
        {
            ProductSectionHint = "Поки триває редагування складу, керування товарами тимчасово недоступне.";
            return;
        }

        if (IsAddNewProductMode)
        {
            ProductSectionHint = "Заповніть форму нового товару та збережіть його в поточний склад.";
            return;
        }

        if (IsAddExistingProductMode)
        {
            ProductSectionHint = "Оберіть уже створений товар, вкажіть кількість для цього складу та збережіть зміни.";
            return;
        }

        ProductSectionHint = "Можна створити новий товар або додати вже існуючий товар у поточний склад з іншою кількістю.";
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
        DeleteWarehouseCommand.RaiseCanExecuteChanged();
        RefreshCommand.RaiseCanExecuteChanged();
        AddNewProductCommand.RaiseCanExecuteChanged();
        AddExistingProductCommand.RaiseCanExecuteChanged();
        SaveNewProductCommand.RaiseCanExecuteChanged();
        SaveExistingProductCommand.RaiseCanExecuteChanged();
        CancelAddProductCommand.RaiseCanExecuteChanged();
        OpenProductCommand.RaiseCanExecuteChanged();
        DeleteSelectedProductCommand.RaiseCanExecuteChanged();
    }

    private enum AddProductMode
    {
        None,
        New,
        Existing
    }
}
