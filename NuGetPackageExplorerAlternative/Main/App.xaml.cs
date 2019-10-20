using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NuGetPackageExplorerAlternative
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void AppStart(object sender, EventArgs e) {
            MainWindow window = new MainWindow();
            window.DataContext = new MainWindowViewModel();
            
            App.Current.MainWindow = window;
            window.Show();
        }
    }
}
