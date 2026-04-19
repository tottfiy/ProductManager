using UIModels.ViewModels.Pages;

namespace UIModels.Navigation;

public class NavigationService : INavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<HomePageViewModel> _homeViewModelFactory;
    private readonly Func<int?, WarehouseDetailsPageViewModel> _warehouseViewModelFactory;
    private readonly Func<int?, int?, ProductDetailsPageViewModel> _productViewModelFactory;

    public NavigationService(
        NavigationStore navigationStore,
        Func<HomePageViewModel> homeViewModelFactory,
        Func<int?, WarehouseDetailsPageViewModel> warehouseViewModelFactory,
        Func<int?, int?, ProductDetailsPageViewModel> productViewModelFactory)
    {
        _navigationStore = navigationStore;
        _homeViewModelFactory = homeViewModelFactory;
        _warehouseViewModelFactory = warehouseViewModelFactory;
        _productViewModelFactory = productViewModelFactory;

        _navigationStore.CurrentViewModelChanged += HandleCurrentViewModelChanged;
    }

    public bool CanGoBack => _navigationStore.CanGoBack;

    public event Action? NavigationChanged;

    public void ShowHome(bool clearHistory = false)
    {
        HomePageViewModel homeViewModel = _homeViewModelFactory();

        if (clearHistory)
        {
            _navigationStore.ClearAndNavigate(homeViewModel);
            return;
        }

        _navigationStore.Navigate(homeViewModel);
    }

    public void ShowWarehouseDetails(int? warehouseId = null)
    {
        _navigationStore.Navigate(_warehouseViewModelFactory(warehouseId));
    }

    public void ShowProductDetails(int? productId = null, int? warehouseId = null)
    {
        _navigationStore.Navigate(_productViewModelFactory(productId, warehouseId));
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
