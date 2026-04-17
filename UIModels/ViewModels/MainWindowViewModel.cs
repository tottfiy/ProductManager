using UIModels.Navigation;

namespace UIModels.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(NavigationStore navigationStore, INavigationService navigationService)
    {
        _navigationStore = navigationStore;
        _navigationService = navigationService;

        _navigationStore.CurrentViewModelChanged += HandleCurrentViewModelChanged;
        _navigationService.NavigationChanged += HandleNavigationChanged;

        _navigationService.ShowHome(clearHistory: true);
    }

    public ViewModelBase? CurrentPageViewModel => _navigationStore.CurrentViewModel;

    private void HandleCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentPageViewModel));
    }

    private void HandleNavigationChanged()
    {
        OnPropertyChanged(nameof(CurrentPageViewModel));
    }
}
