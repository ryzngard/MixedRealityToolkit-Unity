using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    public class PackageManager
    {
        private readonly Project project;
        private readonly PackageManagerSettings settings;

        private IEnumerable<RepositorySettings> enabledRepositories => this.settings.RepositorySettings.Where(r => r.Enabled);

        public PackageManager(Project project, PackageManagerSettings settings)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void DownloadPackage(string name, string version)
        {
            var package = GetPackages().FirstOrDefault(p => p.Name == name && p.Version == version);
            if (package == default(PackageManifest))
            {
                throw new PackageNotFoundException(name, version);
            }

            if (this.settings.EnableLocalCache)
            {
                var path = GetCachePath(name, version);

                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                    Directory.CreateDirectory(path);
                }


            }
            else
            {

            }
        }

        public IEnumerable<PackageManifest> GetPackages()
        {
            foreach (var repo in enabledRepositories)
            {
                using (var disposable = GetStreamFromRepo(repo, out Stream stream))
                {
                    if (stream == null)
                    {
                        throw new InvalidOperationException($"Failed reading '{repo.Name}' at '{repo.Location}'");
                    }

                    var manifests = Serializers.Read<List<PackageManifest>>(stream);

                    foreach (var manifest in manifests)
                    {
                        yield return manifest;
                    }

                    if (disposable != stream)
                    {
                        stream.Dispose();
                    }
                }
            }
        }

        public bool IsPackageInstalled(string name, string version)
        {
            var installed = GetLocalInstalledPackages().FirstOrDefault(p => p.Name == name);
            return installed != default(PackageVersion) && installed.Version != version;
        }

        public bool IsPackageInCache(string name, string version)
        {
            if (!this.settings.EnableLocalCache)
            {
                return false;
            }

            return Directory.Exists(GetCachePath(name, version));
        }

        public void InstallPackage(string name, string version)
        {
            if (IsPackageInstalled(name, version))
            {
                return;
            }
        }

        public void UninstallPackage(string name, string version)
        {
            if (!IsPackageInstalled(name, version))
            {
                return;
            }
        }

        public void EnsurePackages()
        {
            foreach (var package in this.project.Dependencies)
            {
                EnsurePackage(package.Name, package.Version);
            }
        }

        public void EnsurePackage(string name, string version)
        {
            if (!IsPackageInstalled(name, version))
            {
                InstallPackage(name, version);
            }
        }

        public IEnumerable<PackageVersion> UpdatesAvailable()
        {
            var installed = GetLocalInstalledPackages().ToDictionary(p => p.Name);
            List<PackageVersion> updates = new List<PackageVersion>();

            foreach (var manifest in GetPackages())
            {
                if (installed.TryGetValue(manifest.Name, out PackageVersion version))
                {
                    if (version < manifest.PackageVersion)
                    {
                        updates.Add(manifest.PackageVersion);
                    }
                }
            }

            return updates;
        }

        public PackageManifest GetManifest(string name, string version)
        {
            return GetPackages().FirstOrDefault(p => p.Name == name && p.Version == version);
        }

        private IEnumerable<PackageVersion> GetLocalInstalledPackages()
        {
            foreach (var dependency in this.project.Dependencies)
            {
                var path = GetPath(dependency);

                if (Directory.Exists(path))
                {
                    yield return dependency;
                }
            }
        }

        private string GetPath(PackageVersion version)
        {
            return Path.Combine(this.settings.InstallLocation, version.Name, version.Version);
        }

        private string GetCachePath(string name, string version)
        {
            if (!this.settings.EnableLocalCache)
            {
                throw new InvalidOperationException();
            }

            return Path.Combine(this.settings.LocalCacheLocation, name, version);
        }

        private static IDisposable GetStreamFromRepo(RepositorySettings repo, out Stream stream)
        {
            if (repo.Location.IsFile)
            {
                stream = File.Open(repo.Location.AbsolutePath, FileMode.Open);
                return stream;
            }

            switch (repo.Location.Scheme)
            {
                case "file":
                    {
                        stream = File.Open(repo.Location.AbsolutePath, FileMode.Open);
                        return stream;
                    }

                case "http":
                case "https":
                    {
                        var httpRequest = WebRequest.Create(repo.Location.AbsoluteUri);
                        var response = (HttpWebResponse)httpRequest.GetResponse();
                        stream = response.GetResponseStream();

                        return response;
                    }
            }

            throw new InvalidOperationException($"Unable to handle resource for '{repo.Name}' at '{repo.Location}'");
        }
    }
}
