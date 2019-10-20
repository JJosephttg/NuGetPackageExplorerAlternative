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
                IsEditable = false;

                var sourceUri = source as string;
                _sourceUri = sourceUri;

                _packageFinder.InitializeQuerySource(sourceUri);

                var data = await _packageFinder.LoadNextPackageSet();
                foreach (var item in data) NuGetPackageResults.Add(item);

                OnPropertyChanged("NuGetPackageResults");
                IsEditable = true;
            });

            LoadNextPackageSet = new CustomCommand(
                (p) => _packageFinder.HasMoreData,
                async (p) => {
                    IsEditable = false;

                    var data = await _packageFinder.LoadNextPackageSet();
                    foreach (var item in data) NuGetPackageResults.Add(item);

                    OnPropertyChanged("NuGetPackageResults");
                    IsEditable = true;
                }
            );
        }

        public ICommand ChangeSourceCommand { get; }
        public ICommand LoadNextPackageSet { get; }
        public string[] Sources { get; } = new string[] { "https://api.nuget.org/v3/index.json", "http://PromessPackageServer/MicroSoft/nuget" };
        public ObservableCollection<object> NuGetPackageResults { get; private set; } = new ObservableCollection<object>();

        bool _isEditable = true;
        public bool IsEditable {
            get { return _isEditable; }
            private set {
                if (_isEditable != value) {
                    _isEditable = value;
                    OnPropertyChanged("IsEditable");

                    if (_isEditable) NuGetPackageResults.Remove(this);
                    else NuGetPackageResults.Add(this);
                    OnPropertyChanged("NuGetPackageResults");
                }
            }
        }
    }
}
