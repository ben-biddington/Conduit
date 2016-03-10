using System;
using System.IO;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Restoring
{
    public class Can_restore_packages : RunsInCleanRoom
    {
        [Fact]
        public void install_packages_with_dependencies()
        {
            var targetDir = new DirectoryInfo($"packages-{Guid.NewGuid()}");

            FileMachine.Make("packages.config", @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                    <package id=""Conduit.Build.Targets"" version=""0.0.8"" targetFramework=""net45"" />
                </packages>");

            new Build.Targets.Nuget.Install
            {
                NugetUrl            = Settings.PublicNuget.AbsoluteUri,
                TargetDirectory     = targetDir.FullName,
                PackagesConfigFile  = "packages.config",
                Log                 = _ => { },
                IncludeDependencies = true
            }.Execute();

            Assert.True(Directory.Exists(Path.Combine(targetDir.FullName, "Conduit.Build.Targets.0.0.8")));
            Assert.True(Directory.Exists(Path.Combine(targetDir.FullName, "Minimatch.1.1.0.0")));
        }

        [Fact]
        public void or_without()
        {
            var targetDir = new DirectoryInfo($"packages-{Guid.NewGuid()}");

            FileMachine.Make("packages.config", @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                    <package id=""Conduit.Build.Targets"" version=""0.0.8"" targetFramework=""net45"" />
                </packages>");

            new Build.Targets.Nuget.Install
            {
                NugetUrl            = Settings.PublicNuget.AbsoluteUri,
                TargetDirectory     = targetDir.FullName,
                PackagesConfigFile  = "packages.config",
                Log                 = _ => { },
                IncludeDependencies = false
            }.Execute();

            Assert.True(Directory.Exists(Path.Combine(targetDir.FullName, "Conduit.Build.Targets.0.0.8")));
            Assert.False(Directory.Exists(Path.Combine(targetDir.FullName, "Minimatch.1.1.0.0")), "Expected the dependency to be omitted");
        }
    }
}
