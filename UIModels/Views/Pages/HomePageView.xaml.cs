using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UIModels.ViewModels;

namespace UIModels.Views.Pages;

public partial class HomePageView : UserControl
{
    private object? _lastLoadedContext;

    public HomePageView()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await LoadCurrentAsync();
    }

    private async void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (!ReferenceEquals(e.OldValue, e.NewValue))
        {
            _lastLoadedContext = null;
        }

        await LoadCurrentAsync();
    }

    private async Task LoadCurrentAsync()
    {
        if (DataContext is not IAsyncLoadable asyncLoadable)
        {
            return;
        }

        if (ReferenceEquals(_lastLoadedContext, DataContext))
        {
            return;
        }

        _lastLoadedContext = DataContext;
        await asyncLoadable.LoadAsync();
    }
}
