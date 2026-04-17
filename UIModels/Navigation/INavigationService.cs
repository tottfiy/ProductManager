namespace UIModels.Navigation;

public interface INavigationService
{
    bool CanGoBack { get; }
    event Action? NavigationChanged;
    void ShowHome(bool clearHistory = false);
    void ShowWarehouseDetails(int warehouseId);
    void ShowProductDetails(int productId);
    void GoBack();
}
