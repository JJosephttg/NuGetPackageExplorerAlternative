using System;
using System.Windows;

namespace NuGetPackageExplorerAlternative
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void AppStart(object sender, EventArgs e) {
            MainWindow window = new MainWindow(new MainWindowViewModel());
            
            Current.MainWindow = window;
            window.Show();
        }
    }
}
