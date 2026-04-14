using System.Windows;

namespace UIModels
{
    // навігація мід сторінками
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.HomePage());
        }
    }
}