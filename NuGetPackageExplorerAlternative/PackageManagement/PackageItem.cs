using NuGet.Protocol.Core.Types;

namespace NuGetPackageExplorerAlternative {
    public class PackageItem {
        public PackageItem(IPackageSearchMetadata metadata, string version) {
            Metadata = metadata;
            Version = version;
        }

        public IPackageSearchMetadata Metadata { get; private set; }
        public string Version { get; private set; }
    }
}
