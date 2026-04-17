using UIModels.ViewModels.Pages;

namespace UIModels.Navigation;

public class NavigationService : INavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<int, WarehouseDetailsPageViewModel> _warehouseViewModelFactory;
    private readonly Func<int, ProductDetailsPageViewModel> _productViewModelFactory;

    public NavigationService(
        NavigationStore navigationStore,
        IServiceProvider serviceProvider,
        Func<int, WarehouseDetailsPageViewModel> warehouseViewModelFactory,
        Func<int, ProductDetailsPageViewModel> productViewModelFactory)
    {
        _navigationStore = navigationStore;
        _serviceProvider = serviceProvider;
        _warehouseViewModelFactory = warehouseViewModelFactory;
        _productViewModelFactory = productViewModelFactory;

        _navigationStore.CurrentViewModelChanged += HandleCurrentViewModelChanged;
    }

    public bool CanGoBack => _navigationStore.CanGoBack;
    public event Action? NavigationChanged;

    public void ShowHome(bool clearHistory = false)
    {
        var viewModel = (HomePageViewModel)_serviceProvider.GetService(typeof(HomePageViewModel))!;

        if (clearHistory)
        {
            _navigationStore.ClearAndNavigate(viewModel);
            return;
        }

        _navigationStore.Navigate(viewModel);
    }

    public void ShowWarehouseDetails(int warehouseId)
    {
        _navigationStore.Navigate(_warehouseViewModelFactory(warehouseId));
    }

    public void ShowProductDetails(int productId)
    {
        _navigationStore.Navigate(_productViewModelFactory(productId));
    }

    public void GoBack()
    {
        _navigationStore.GoBack();
    }

    private void HandleCurrentViewModelChanged()
    {
        NavigationChanged?.Invoke();
    }
}
