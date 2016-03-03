using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class Nuget
    {
        public static List<FileInfo> Flatten(Uri uri, DirectoryInfo packageDirectory, NugetPackage p, DirectoryInfo targetDirectory)
        {
            var package = new PackageManager(PackageRepository(uri), packageDirectory.FullName).LocalRepository.FindPackage(p.Id);

            if (null == package)
                return new List<FileInfo>(0);

            var packageFiles = package.GetFiles().ToList();

            var packagePath = Path.Combine(packageDirectory.FullName, string.Join(".", package.Id, package.Version));

            var matchingFiles = packageFiles.
                Where(it => it.TargetFramework.Version.ToString().Equals(p.Version.Version, StringComparison.InvariantCultureIgnoreCase)).
                Select(it => new FileInfo(Path.Combine(packagePath, it.Path))).ToList();

            Ensure(targetDirectory);

            matchingFiles.ForEach(it => it.CopyTo(Path.Combine(targetDirectory.FullName, it.Name)));

            return matchingFiles;
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

        public static void Install(Uri uri, string id, DirectoryInfo directory)
        {
            Install(uri, id, null, directory);
        }

        public static void Install(Uri uri, string id, string version, DirectoryInfo directory)
        {
            var semanticVersion = version != null ? SemanticVersion.Parse(version) : null;

            new PackageManager(PackageRepository(uri), directory.FullName).InstallPackage(id, semanticVersion);
        }

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }
    }

    public class NugetPackage
    {
        public string Id { get; private set; }
        public FrameworkVersion Version { get; private set; }

        public NugetPackage(string id) : this(id, null)
        {
        }

        public NugetPackage(string id, FrameworkVersion version)
        {
            Id = id;
            Version = version;
        }
    }
}
