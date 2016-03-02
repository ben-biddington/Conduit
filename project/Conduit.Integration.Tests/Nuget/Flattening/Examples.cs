using System;
using System.IO;
using System.Threading;
using Conduit.Adapters.Build;
using Conduit.Integration.Tests.Support;
using Xunit;
using NuGet;
using System.Collections.Generic;
using System.Linq;

namespace Conduit.Integration.Tests
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
            FileMachine.Make("packages.config", @"
                <?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                    <package id=""MSBuild.Extension.Pack"" version=""1.8.0"" />
                </packages>");

            string packageID = "EntityFramework";

            var repo = NuGet.PackageRepositoryFactory.Default.CreateRepository ("https://packages.nuget.org/api/v2");

            var packages = repo.FindPackagesById(packageID).ToList();

            Assert.True (packages.Count > 0);
        }
    }

    internal static class Nugget {
        internal static FileInfo[] Flatten(DirectoryInfo packageDirectory, FrameworkVersion version, DirectoryInfo targetDirectory) 
        {

            return targetDirectory.GetFiles();
        }
    }

    public class FrameworkVersion {
        public static readonly FrameworkVersion Net45 = new FrameworkVersion("net45") ;
        public string Name { get; private set;}
        public FrameworkVersion(string name)
        {
            Name = name;
        }
    }
}

