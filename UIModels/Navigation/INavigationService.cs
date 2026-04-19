using UIModels.ViewModels;

namespace UIModels.Navigation;

public interface INavigationService
{
    bool CanGoBack { get; }
    event Action? NavigationChanged;
    void ShowHome(bool clearHistory = false);
    void ShowWarehouseDetails(int? warehouseId = null);
    void ShowProductDetails(int? productId = null, int? warehouseId = null);
    void GoBack();
}
