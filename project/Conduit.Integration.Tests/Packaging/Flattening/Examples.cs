using System;
using System.IO;
using System.Linq;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Flattening
{
    public class Examples : RunsInCleanRoom
    {
        [Fact]
        public void the_basics() 
        {
            var packages = Nuget.Find(new Uri("https://packages.nuget.org/api/v2"), "EntityFramework").ToList();

            Assert.True(packages.Count > 0);
        }

        [Fact]
        public void install_it()
        {
            var targetDir = new DirectoryInfo($"packages-{Guid.NewGuid()}");

            Nuget.Install(new Uri("https://packages.nuget.org/api/v2"), targetDir, new NugetPackage("Conduit.Build.Targets", new PackageVersion("0.0.8"), FrameworkVersion.Net45));

            Assert.True(targetDir.Exists);
        }
    }
}

