using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace NuGetPackageExplorerAlternative {
    public class PackageFinder {
        const int C_QuerySize = 20;

        string _uri;
        int _startIndex = 0;
        public void InitializeQuerySource(string uri) {
            _uri = uri;
            _startIndex = 0;
        }

        public async Task<List<PackageItem>> LoadNextPackageSet() {
            var list = new List<PackageItem>();
            if (string.IsNullOrWhiteSpace(_uri)) return list;

            PackageSource packageSource = new PackageSource(_uri);
            SourceRepository sourceRepo = new SourceRepository(packageSource, Repository.Provider.GetCoreV3());
            PackageSearchResource searchResource = await sourceRepo.GetResourceAsync<PackageSearchResource>();

            IEnumerable<IPackageSearchMetadata> data = await searchResource.SearchAsync(null, new SearchFilter(true), _startIndex, C_QuerySize, NullLogger.Instance, CancellationToken.None);
            foreach (var item in data) list.Add(new PackageItem(item, item.Identity.Version.ToString()));
            _startIndex += C_QuerySize;
            return list;
        }
    }
}
