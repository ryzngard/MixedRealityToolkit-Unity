# Package Manager

The package manager is intended to highlight what is available from both Microsoft and the community for Mixed Reality Toolkit. It will suplement Unity's built in package management that is available by highlighting [vetted](#package-vetting) resources for developers.

## Package Vetting

Package vetting for showing up in the package manager UI should be done in multiple ways. Naming is subject to change, but the below outlines the general split on how packages will be presented. 

The main goal is that all packages are contained within the Mixed Reality Toolkit Repo, or some separate repo mantained by the same community. This brings clarity to users about where issues can be filed, where to find more information about specific packages, and how they can get their packages added. This will explicitly not display any packages that are not contained within these constraints; while they may be good packages that work well with Mixed Reality Toolkit, they are not mantained within the community and will not be promoted in the same way. Instead, it will be up to the package owners to promote it's use in another way. We may find other promotion techniques if we as a community believe a package provides value but is not part of our ecosystem. 

### Release Packages

Release packages are promoted and mantained with some promise of stability and support (TBD). These will be packages like Azure integration that we believe work well with Mixed Reality Toolkit and explicitly provide features that Microsoft is promoting. These packages will also have a strict process for submitting code changes.

### Preview Packages

These packages contain less promise to quality and maintainance, but still exist within the ecosystem. The process for code changes will be much less stringent. However, we still want to celebrate community contributions and will surface packages like this in the package manager UI. 

# Classes

The classes below are intended to represent the package manager as a standalone tool that MRTK uses. The tool could potentially reside in another repo and be consumed at regular intervals inside MRTK. This allows us to not only publish the package manager as a standalone tool for others if they need, but keeps it contained for discussions and bug reporting. It also keeps a very clean line betwen MRTK and Package Management in the event that we need to switch to the standard Unity system.

## PackageManager

### Constructors

|     |     | 
| --- | --- | 
| PackageManager(Project, PackageManagerSettings) | Initializes a new instance with the specified [Project](#project) and [PackageManagerSettings](#packagemanagersettings) |

### Methods

| Name | Return Type | Description |
| ---- | ----------- | ----------- |
| DownloadPackage(string, string) | void | Downloads the package to the local cache if enabled |
| GetPackages() | IEnumerable\<[PackageManifest](#packagemanifest)> | Queries the enabled repositories for all available packages |
| IsPackageInstalled(string, string) | bool | Returns true if the package is referenced by the project and currently installed |
| IsPackageInCache(string, string) | bool | Returns true if the cache is enabled, and the package already exists |
| InstallPackage(string, string) | void | Installs the package. If the package cache is enabled, it will check there and make sure the cache is populated before installing to the local project |
| UninstallPackage(string, string) | void | Uninstalls a package. No-op is the package is not currently installed. Does not remove from the package cache if the cache is enabled | 
| EnsurePackages() | void | Makes sure all dependencies of the [Project](#project) are installed | 
| EnsurePackage(string, string) | void | Like above, but only for a single package | 
| UpdatesAvailable() | IEnumerable\<[PackageVersion](#packageversion)> | Gets all packages that have updates available by querying the repositories | 
| GetManifest(string, string) | [PackageManifest](#packagemanifest) | Queries the repositories for a specific package |

## Project 

### Constructors

|     |     |
| --- | --- |
| Project() | Default constructor | 

### Properties 

| Name | Type | Description |
| ---- | ---- | ----------- |
| Dependencies | IList\<[PackageVersion](#packageversion)> | The list of dependencies for this project

## PackageManagerSettings

### Constructors

|     |     |
| --- | --- |
| PackageManagerSettings() | Default constructor | 

### Properties

| Name | Type | Description |
| ---- | ---- | ----------- |
| RepositorySettings | IList\<[RepositorySettings](#repositorySettings)> | List of repository settings the package manager will use |
| LocalCacheLocation | string | Absolute or relative path to where the package cache will be | 
| EnableLocalCache | bool | Enables or disables the local cache. On by default. |
| InstallLocation | string | Location to install packages for use in a project |

## PackageManifest

A manifest that describes all metadata about a package. Attempts to mantain compatability with [Unity Package Manifests](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html) for easy porting in the future.

### Constructors

|     |     |
| --- | --- |
| PackageManifest() | Default constructor |

### Properties

| Name | Type | Description |
| ---- | ---- | ----------- |
| Name | string | The fully qualified name of the package |
| DisplayName | string | The friendly readable name of the package | 
| Version | string | The version, following [SemVer v2](https://semver.org/spec/v2.0.0.html) semantics |
| Description | string | A description of the package |
| Keywords | IList\<string> | The keywords for the package | 
| Category | string | The category of the package | 
| Dependencies | IList\<[PackageVersion](#packageversion)> | The packages that this one depends on |

## PackageVersion

A shortened reference to a specific package and version pairing without the full manifest.

### Constructors

|     |     |
| --- | --- |
| PackageVersion() | Default constructor |

### Properties

| Name | Type | Description |
| ---- | ---- | ----------- |
| Name | string | The fully qualified name of the package |
| Version | string | The version, following [SemVer v2](https://semver.org/spec/v2.0.0.html) semantics |