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
            var repo = PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);

            return repo.FindPackagesById(id).Select(it => new NugetPackage(it.Id));
        }

        public static void Install(Uri uri, string id, string version, DirectoryInfo directory)
        {
            var repo = PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);

            new PackageManager(repo, directory.FullName).InstallPackage(id, SemanticVersion.Parse(version));
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
