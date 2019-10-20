using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace NuGetPackageExplorerAlternative {
    public class PackageFinder {
        const int C_QuerySize = 20;

        string _uri;
        int _startIndex = 0;
        PackageSearchResource _searchResource;
        SearchFilter _searchFilter = new SearchFilter(true);
        public PackageFinder(string uri) {
            _uri = uri;
            _startIndex = 0;
            HasMoreData = true;
        }

        public async Task<List<PackageItem>> LoadNextPackageSet(CancellationToken token) {
            var list = new List<PackageItem>();
            if (string.IsNullOrWhiteSpace(_uri)) return list;

            IPackageSearchMetadata[] data;
            try {
                if (_searchResource == null) {
                    PackageSource packageSource = new PackageSource(_uri);
                    SourceRepository sourceRepo = new SourceRepository(packageSource, Repository.Provider.GetCoreV3());
                    _searchResource = await sourceRepo.GetResourceAsync<PackageSearchResource>(token);
                }

                data = (await _searchResource.SearchAsync(null, _searchFilter, _startIndex, C_QuerySize, NullLogger.Instance, token)).ToArray();
                
                token.ThrowIfCancellationRequested();
            } catch (TaskCanceledException) {
                return list;
            }

            foreach (var item in data) list.Add(new PackageItem(item));

            HasMoreData = data.Length == C_QuerySize;
            _startIndex += C_QuerySize;
            return list;
        }

        public bool HasMoreData { get; private set; } = false;
    }
}
