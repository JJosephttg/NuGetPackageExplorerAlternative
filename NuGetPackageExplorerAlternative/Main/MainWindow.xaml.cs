using System.Windows;
using System.Windows.Controls;

namespace NuGetPackageExplorerAlternative
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _vm;
        public MainWindow(MainWindowViewModel vm) {
            _vm = vm;
            DataContext = vm;
            InitializeComponent();
        }

        private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.VerticalChange != 0 && e.OriginalSource is ScrollViewer scrollViewer && scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                if (scrollViewer.VerticalOffset == 0 && scrollViewer.ScrollableHeight == 0) // scroll back to top if packages got cleared
                    scrollViewer.ScrollToTop();
                else if (_vm.LoadNextPackageSet.CanExecute(null)) // load more packages if scrolled to end
                    _vm.LoadNextPackageSet.Execute(null);
            else if (e.ExtentHeight > 0 && e.ExtentHeight < e.ViewportHeight) // load more packages if viewport is higher than used space
                if (_vm.LoadNextPackageSet.CanExecute(null))
                    _vm.LoadNextPackageSet.Execute(null);
        }
    }
}
