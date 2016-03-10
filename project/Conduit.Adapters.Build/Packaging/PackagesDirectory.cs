using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class PackagesDirectory
    {
        public static List<NugetPackage> Read(DirectoryInfo packageDirectory)
        {
            return new PackageManager(PackageRepository(new Uri("http://xxx-not-required")), packageDirectory.FullName).LocalRepository.GetPackages().
                Select(it => new NugetPackage(it.Id, new PackageVersion(it.Version.ToString()), FrameworkNames.Net45)).ToList();
        }

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }
    }
}