using McMaster.Extensions.CommandLineUtils;
using Microsoft.MixedReality.Toolkit.PackageManager;
using System;

namespace Package_Manager_Cmd
{
    public enum Command
    {
        InstallPackage,
        UninstallPackage,
        ListInstalledPackages,
        RestorePackages
    }

    class Program
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);


        [Argument(0, Description = "Command to execute")]
        public Command Command { get; }

        [Option("-p|--project", Description = "The project file to target")]
        public string Project { get; }

        [Option("-n|--name", Description = "Name of the package")]
        public string PackageName { get; }

        [Option("-v|--version", Description = "Version of the package")]
        public string Version { get; }

        [Option("-s|--settings", Description = "Path to package manager settings file")]
        public string PackageManagerSettings { get; }

        private void OnExecute()
        {
            switch(Command)
            {
                case Command.InstallPackage:
                    InstallPackage();
                    break;

                case Command.UninstallPackage:
                    UninstallPackage();
                    break;

                case Command.ListInstalledPackages:
                    ListInstalledPackages();
                    break;

                case Command.RestorePackages:
                    RestorePackages();
                    break;

                default:
                    throw new NotImplementedException($"{Command} is not implemented");
            }
        }

        private void InstallPackage()
        {
            var version = EnsureVersion();
            var name = EnsurePackageName();
            var project = GetProject();
            var settings = GetSettings();

            var manager = new PackageManager(project, settings);
            manager.InstallPackage(name, version);
        }

        private void UninstallPackage()
        {
            var version = EnsureVersion();
            var name = EnsurePackageName();
            var project = GetProject();
            var settings = GetSettings();

            var manager = new PackageManager(project, settings);
            manager.UninstallPackage(name, version);
        }

        private void ListInstalledPackages()
        {
            var project = GetProject();
            var settings = GetSettings();

            var manager = new PackageManager(project, settings);
            var packages = manager.GetPackages();
        }

        private void RestorePackages()
        {
            var project = GetProject();
            var settings = GetSettings();

            var manager = new PackageManager(project, settings);
            manager.EnsurePackages();
        }

        private PackageManagerSettings GetSettings()
        {
            if (string.IsNullOrEmpty(this.PackageManagerSettings))
            {
                throw new NotImplementedException("No default package manager settings provided, please provide path to settings");
            }

            return Serializers.Read<PackageManagerSettings>(this.PackageManagerSettings);
        }

        private Project GetProject()
        {
            if (string.IsNullOrEmpty(this.Project))
            {
                throw new NotImplementedException("Project must be specified");
            }

            return Serializers.Read<Project>(this.Project);
        }

        private string EnsureVersion()
        {
            if (string.IsNullOrEmpty(this.Version))
            {
                throw new ArgumentNullException("Version is a required parameter");
            }

            return this.Version;
        }

        private string EnsurePackageName()
        {
            if(string.IsNullOrEmpty(this.PackageName))
            {
                throw new ArgumentNullException("Name is a required paramter");
            }

            return this.PackageName;
        }
    }
}
