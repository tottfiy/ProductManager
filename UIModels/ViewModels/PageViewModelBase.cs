using System.Windows;

namespace UIModels.ViewModels;

public abstract class PageViewModelBase : ViewModelBase, IAsyncLoadable
{
    private bool _isBusy;
    private string _busyMessage = string.Empty;

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
    }

    public bool IsNotBusy => !IsBusy;

    public string BusyMessage
    {
        get => _busyMessage;
        private set => SetProperty(ref _busyMessage, value);
    }

    public abstract Task LoadAsync();

    protected async Task ExecuteBusyActionAsync(string busyMessage, Func<Task> action)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        BusyMessage = busyMessage;
        RaiseCommandStates();

        try
        {
            await action();
        }
        finally
        {
            BusyMessage = string.Empty;
            IsBusy = false;
            RaiseCommandStates();
        }
    }

    protected void ShowError(string message)
    {
        MessageBox.Show(message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    protected void ShowInfo(string message)
    {
        MessageBox.Show(message, "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    protected static void ReplaceCollection<T>(ICollection<T> targetCollection, IEnumerable<T> newItems)
    {
        targetCollection.Clear();
        foreach (T item in newItems)
        {
            targetCollection.Add(item);
        }
    }

    protected virtual void RaiseCommandStates()
    {
    }
}
