using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NuGetPackageExplorerAlternative {
    public class MainWindowViewModel : ViewModelBase {
        PackageFinder _packageFinder;
        string _sourceUri = "";
        public MainWindowViewModel() {
            _packageFinder = new PackageFinder();

            ChangeSourceCommand = new CustomCommand(async (source) => {
                NuGetPackageResults.Clear();
                var sourceUri = source as string;
                _sourceUri = sourceUri;

                _packageFinder.InitializeQuerySource(sourceUri);

                var data = await _packageFinder.LoadNextPackageSet();
                
                foreach (var item in data)
                    NuGetPackageResults.Add(item);

                OnPropertyChanged("NuGetPackageResults");
            });
        }

        public ICommand ChangeSourceCommand { get; }
        public string[] Sources { get; } = new string[] { "https://api.nuget.org/v3/index.json", "http://PromessPackageServer/MicroSoft/nuget" };
        public ObservableCollection<PackageItem> NuGetPackageResults { get; private set; } = new ObservableCollection<PackageItem>();
    }
}
