using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class Nuget
    {
        public static List<FileInfo> Flatten(Uri uri, DirectoryInfo packageDirectory, string id, FrameworkVersion version, DirectoryInfo targetDirectory)
        {
            var package = new PackageManager(PackageRepository(uri), packageDirectory.FullName).LocalRepository.FindPackage(id);

            if (null == package)
                return new List<FileInfo>(0);

            Ensure(targetDirectory);

            var packageFiles = package.GetFiles().ToList();

            var matchingFiles = packageFiles.
                Where(it => it.TargetFramework.Version.ToString().Equals(version.Version, StringComparison.InvariantCultureIgnoreCase));

            return matchingFiles.Select(it => new FileInfo(it.Path)).ToList();
        }

        private static void Ensure(DirectoryInfo targetDirectory)
        {
            if (false == targetDirectory.Exists)
            {
                targetDirectory.Create();
            }
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
