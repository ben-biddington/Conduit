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
        /*

        The idea here is to allow selection of a bunch of assemblies from a set of nuget packages so that you can put them all in one directory.

        This means all dependencies can be satisfied.

        IDEs call this "copy local"

        */

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

            Nuget.Install(new Uri("https://packages.nuget.org/api/v2"), "Conduit.Build.Targets", "0.0.8", targetDir);

            Assert.True(targetDir.Exists);
        }
    }
}

