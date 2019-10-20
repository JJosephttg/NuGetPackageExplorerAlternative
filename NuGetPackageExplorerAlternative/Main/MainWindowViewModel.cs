using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuGetPackageExplorerAlternative {
    public class MainWindowViewModel : ViewModelBase {
        PackageFinder _packageFinder;

        public MainWindowViewModel() {
            ChangeSourceCommand = new CustomCommand(async (source) => {
                NuGetPackageResults.Clear();

                _packageFinder = new PackageFinder(source as string);

                await LoadNextPackageChunk();
                SelectedPackage = NuGetPackageResults.Count > 0 ? NuGetPackageResults[0] as PackageItem : null;
            });

            LoadNextPackageSet = new CustomCommand(
                (p) => _packageFinder.HasMoreData && IsEditable,
                async (p) => { await LoadNextPackageChunk(); }
            );

            CancelCommand = new CustomCommand(
                (p) => !IsEditable && CurrentCancellationToken != null,
                (p) => {
                    CurrentCancellationToken.Cancel();
                    IsEditable = true;
                }
            );

            ChangeSourceCommand.Execute(Sources[1]);
        }

        public ICommand ChangeSourceCommand { get; }
        public ICommand LoadNextPackageSet { get; }
        public ICommand CancelCommand { get; }
        public string[] Sources { get; } = new string[] { "https://api.nuget.org/v3/index.json", "http://PromessPackageServer/MicroSoft/nuget" };
        public ObservableCollection<object> NuGetPackageResults { get; } = new ObservableCollection<object>();

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

        string _error;
        public string ErrorMessage {
            get { return _error; }
            private set {
                _error = value;
                OnPropertyChanged("ErrorMessage");
            }
        }


        PackageItem _selectedPackage;
        public PackageItem SelectedPackage {
            get { return _selectedPackage; }
            set {
                if(_selectedPackage != value) {
                    _selectedPackage = value;
                    OnPropertyChanged("SelectedPackage");
                }
            }
        }

        CancellationTokenSource _currentCancellationToken;
        CancellationTokenSource CurrentCancellationToken {
            get { return _currentCancellationToken; }
            set {
                if(_currentCancellationToken != null)
                    _currentCancellationToken.Cancel();
                _currentCancellationToken = value;
            }
        }

        private async Task LoadNextPackageChunk() {
            ErrorMessage = null;
            IsEditable = false;

            var tokenSource = new CancellationTokenSource();
            CurrentCancellationToken = tokenSource;

            List<PackageItem> data = new List<PackageItem>();
            try {
                data = await _packageFinder.LoadNextPackageSet(tokenSource.Token);
            } catch (Exception e) {
                ErrorMessage = e.Message;
            }

            foreach (var item in data) NuGetPackageResults.Add(item);

            OnPropertyChanged("NuGetPackageResults");
            if (CurrentCancellationToken == tokenSource) CurrentCancellationToken = null;
            tokenSource.Dispose();
            IsEditable = true;
        }
    }
}
