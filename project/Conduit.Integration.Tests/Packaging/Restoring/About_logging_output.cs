using System;
using System.IO;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Restoring
{
    public class About_logging_output : RunsInCleanRoom
    {
        [Fact]
        public void install_packages_with_dependencies()
        {
            var targetDir = new DirectoryInfo($"packages-{Guid.NewGuid()}");

            FileMachine.Make("packages.config", @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                    <package id=""Conduit.Build.Targets"" version=""0.0.8"" targetFramework=""net45"" />
                </packages>");

            var log = new MockLog();

            new Build.Targets.Nuget.Install
            {
                NugetUrl            = Settings.PublicNuget.AbsoluteUri,
                TargetDirectory     = targetDir.FullName,
                PackagesConfigFile  = "packages.config",
                IncludeDependencies = true,
                Log                 = log.Fun()
            }.Execute();

            log.MustHaveMessageLike("Installing package <Conduit.Build.Targets, 0.0.8.0> with dependencies");
            log.MustHaveMessageLike("Installing package <Minimatch, 1.1.0.0>");
            log.MustHaveMessageLike("Installed package <Minimatch, 1.1.0.0>");
            log.MustHaveMessageLike("Installed package <Conduit.Build.Targets, 0.0.8.0>");
        }
    }
}