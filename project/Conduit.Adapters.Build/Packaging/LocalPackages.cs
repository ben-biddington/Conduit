using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class LocalPackages
    {
        public static NugetPackage Find(Uri uri, DirectoryInfo packageDirectory, string id)
        {
            var thirdParty = new PackageManager(PackageRepository(uri), packageDirectory.FullName).LocalRepository.FindPackage(id);

            return Map(thirdParty);
        }

        private static NugetPackage Map(IPackage thirdParty)
        {
            return new NugetPackage(thirdParty.Id, new PackageVersion(thirdParty.Version.ToString()), FrameworkVersion.Net45);
        }

        public static List<NugetPackage> Find(Uri uri, string id)
        {
            return PackageRepository(uri).FindPackagesById(id).Select(it => new NugetPackage(it.Id)).ToList();
        }

        public static void Install(Uri uri, DirectoryInfo directory, params NugetPackage[] packages)
        {
            foreach (var package in packages)
            {
                var semanticVersion = package.Version != null ? SemanticVersion.Parse(package.Version.Value) : null;

                new PackageManager(PackageRepository(uri), directory.FullName).InstallPackage(package.Id, semanticVersion);
            }
        }

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }
    }
}