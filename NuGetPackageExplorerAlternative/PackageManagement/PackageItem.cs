using NuGet.Protocol.Core.Types;

namespace NuGetPackageExplorerAlternative {
    public class PackageItem : ViewModelBase {
        //Getters prevent binding leaks from occurring since IPackageSearchMetadata doesn't implement INotifyPropertyChanged 
        IPackageSearchMetadata _metadata;
        public PackageItem(IPackageSearchMetadata metadata) {
            _metadata = metadata;
        }

        public string IconUrl { get { return _metadata.IconUrl?.ToString(); } }
        public string Id { get { return _metadata.Identity.Id; } }
        public string Summary { get { return _metadata.Summary; } }
        public string Description { get { return _metadata.Description; } }
        public string Authors { get { return _metadata.Authors; } }
        public long DownloadCount { get { return _metadata.DownloadCount ?? 0; } }
        public bool IsPrerelease { get { return _metadata.Identity.Version.IsPrerelease; } }
        public string Version { get { return _metadata.Identity.Version?.ToFullString(); } }
        public string LicenseUrl { get { return _metadata.LicenseUrl?.ToString(); } }
        public string ProjectUrl { get { return _metadata.ProjectUrl?.ToString(); } }
        public string ReportAbuseUrl { get { return _metadata.ReportAbuseUrl?.ToString(); } }
        public string Tags { get { return _metadata.Tags; } }
    }
}
