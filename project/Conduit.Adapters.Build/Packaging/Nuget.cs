using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class Nuget
    {
        public static FileInfo[] Flatten(DirectoryInfo packageDirectory, FrameworkVersion version, DirectoryInfo targetDirectory)
        {
            return targetDirectory.GetFiles();
        }

        public static IEnumerable<NugetPackage> Find(Uri uri, string id)
        {
            return PackageRepository(uri).FindPackagesById(id).Select(it => new NugetPackage(it.Id));
        }

        public static void Install(Uri uri, string id, string version, DirectoryInfo directory)
        {
            new PackageManager(PackageRepository(uri), directory.FullName).InstallPackage(id, SemanticVersion.Parse(version));
        }

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }
    }

    public class NugetPackage
    {
        public string Id { get; private set; }

        public NugetPackage(string id)
        {
            Id = id;
        }
    }
}
