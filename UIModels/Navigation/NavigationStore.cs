using UIModels.ViewModels;

namespace UIModels.Navigation;

public class NavigationStore
{
    private readonly Stack<ViewModelBase> _history = new();
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel => _currentViewModel;
    public bool CanGoBack => _history.Count > 0;

    public event Action? CurrentViewModelChanged;

    public void Navigate(ViewModelBase viewModel, bool addToHistory = true)
    {
        if (_currentViewModel is not null && addToHistory)
        {
            _history.Push(_currentViewModel);
        }

        _currentViewModel = viewModel;
        CurrentViewModelChanged?.Invoke();
    }

    public void ClearAndNavigate(ViewModelBase viewModel)
    {
        _history.Clear();
        _currentViewModel = viewModel;
        CurrentViewModelChanged?.Invoke();
    }

    public void GoBack()
    {
        if (!CanGoBack)
        {
            return;
        }

        _currentViewModel = _history.Pop();
        CurrentViewModelChanged?.Invoke();
    }
}
