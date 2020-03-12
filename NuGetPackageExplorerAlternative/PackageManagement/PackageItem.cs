using NuGet.Protocol.Core.Types;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NuGetPackageExplorerAlternative {
    public class PackageItem : ViewModelBase {
        //Getters prevent binding leaks from occurring since IPackageSearchMetadata doesn't implement INotifyPropertyChanged 
        IPackageSearchMetadata _metadata;

        public PackageItem(IPackageSearchMetadata metadata) {
            _metadata = metadata;
            var url = _metadata.IconUrl?.ToString();
            if (string.IsNullOrWhiteSpace(url)) return;

            BitmapImage maybeIcon = new BitmapImage();
            maybeIcon.DecodePixelWidth = 32;

            RenderOptions.SetBitmapScalingMode(maybeIcon, BitmapScalingMode.HighQuality);
            maybeIcon.BeginInit();
            maybeIcon.DownloadFailed += MaybeIcon_DownloadFailed;
            maybeIcon.DecodeFailed += MaybeIcon_DownloadFailed;
            maybeIcon.UriSource = new Uri(url);
            try {
                maybeIcon.EndInit();
            } catch(Exception) {
                return;
            }
            
            Icon = maybeIcon;
        }

        private void MaybeIcon_DownloadFailed(object sender, ExceptionEventArgs e) {
            if(sender is BitmapImage image) {
                image.DownloadFailed -= MaybeIcon_DownloadFailed;
                image.DecodeFailed -= MaybeIcon_DownloadFailed;
                Icon = new BitmapImage(new Uri("pack://application:,,,/Assets/nuget.png"));
            }
        }

        BitmapImage _icon = new BitmapImage(new Uri("pack://application:,,,/Assets/nuget.png"));
        public BitmapImage Icon { get { return _icon; } private set { _icon = value; OnPropertyChanged("Icon"); } }
        public string Id { get { return _metadata.Identity.Id; } }
        public string Summary { get { return _metadata.Summary; } }
        public string Description { get { return _metadata.Description; } }
        public string Authors { get { return _metadata.Authors; } }
        public long DownloadCount { get { return _metadata.DownloadCount ?? 0; } }
        public bool IsPrerelease { get { return _metadata.Identity.Version.IsPrerelease; } }
        public string Version { get { return _metadata.Identity.Version?.ToFullString(); } }
        public Uri LicenseUrl { 
            get {
                Uri uri;
                var success = Uri.TryCreate(_metadata.LicenseUrl?.ToString(), UriKind.RelativeOrAbsolute, out uri);
                return success ? uri : null; 
            }
        }
        public Uri ProjectUrl {
            get {
                Uri uri;
                var success = Uri.TryCreate(_metadata.ProjectUrl?.ToString(), UriKind.RelativeOrAbsolute, out uri);
                return success ? uri : null;
            }
        }
        public Uri ReportAbuseUrl {
            get {
                Uri uri;
                var success = Uri.TryCreate(_metadata.ReportAbuseUrl?.ToString(), UriKind.RelativeOrAbsolute, out uri);
                return success ? uri : null;
            }
        }
        public string Tags { get { return _metadata.Tags; } }
    }
}
