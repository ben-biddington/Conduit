using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class Nuget
    {
        public static List<FileInfo> Flatten(Uri uri, DirectoryInfo packageDirectory, DirectoryInfo targetDirectory, params NugetPackage[] packages)
        {
            return Private.Flatten.Apply(uri, packageDirectory, targetDirectory, packages);
        }

        public static List<NugetPackage> Find(Uri uri, string id)
        {
            return PackageRepository(uri).FindPackagesById(id).Select(it => new NugetPackage(it.Id)).ToList();
        }

        public static void Install(Uri uri, DirectoryInfo directory, params NugetPackage[] packages)
        {
            Install (uri, directory, new InstallOptions (), packages);
        }

        public static void Install(Uri uri, DirectoryInfo directory, InstallOptions opts, params NugetPackage[] packages)
        {
            foreach (var package in packages)
            {
                var semanticVersion = package.Version != null ? SemanticVersion.Parse(package.Version.Value) : null;

                new PackageManager(PackageRepository(uri), directory.FullName).InstallPackage(package.Id, semanticVersion, false == opts.IncludeDependencies, false);
            }
        }

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }

        public class InstallOptions
        {
            public bool IncludeDependencies { get; private set; }

            public InstallOptions(bool dependencies = false) 
            {
                IncludeDependencies = dependencies;
            }
        }
    }
}
