using System.IO;
using Conduit.Adapters.Build.Packaging;
using Conduit.Build.Targets.Nuget;

namespace Conduit.Integration.Tests.Support
{
    public class AcceptanceTest : RunsInCleanRoom
    {
        private FileInfo _packagesConfigFile;
        private DirectoryInfo _packagesDirectory;

        protected void Given_this_is_present_and_installed_with_dependencies(string xml)
        {
            _packagesConfigFile = FileMachine.Make("packages.config", xml);
            _packagesDirectory = new DirectoryInfo("packages");

            Nuget.Install(Settings.PublicNuget, _packagesDirectory, new Nuget.InstallOptions(true), PackagesConfig.Read(_packagesConfigFile).ToArray());
        }

        protected void Given_this_is_present_and_installed(string xml)
        {
            _packagesConfigFile = FileMachine.Make("packages.config", xml);
            _packagesDirectory  = new DirectoryInfo("packages");

            Nuget.Install(Settings.PublicNuget, _packagesDirectory, PackagesConfig.Read(_packagesConfigFile).ToArray());
        }

        protected void When_I_flatten_to(DirectoryInfo targetDirectory)
        {
            new Flatten
            {
                PackagesConfigFile  = _packagesConfigFile.FullName,
                PackagesDirectory   = _packagesDirectory.FullName,
                TargetDirectory     = targetDirectory.FullName
            }.Execute();
        }
    }
}