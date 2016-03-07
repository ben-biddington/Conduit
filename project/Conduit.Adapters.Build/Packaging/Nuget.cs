using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class Nuget
    {
        public static List<FileInfo> Flatten(Uri uri, DirectoryInfo packageDirectory, DirectoryInfo targetDirectory, params NugetPackage[] packages)
        {
            var result = new List<FileInfo>();

            foreach (var nugetPackage in packages)
            {
                result.AddRange(FlattenSingle(uri, packageDirectory, targetDirectory, nugetPackage));
            }

            return result;
        }

        private static List<FileInfo> FlattenSingle(Uri uri, DirectoryInfo packageDirectory, DirectoryInfo targetDirectory, NugetPackage p)
        {
            var package = new PackageManager(PackageRepository(uri), packageDirectory.FullName).LocalRepository.FindPackage(p.Id);

            if (null == package)
                return new List<FileInfo>(0);

            var packageFiles = package.GetFiles().ToList();

            var packagePath = Path.Combine(packageDirectory.FullName, string.Join(".", package.Id, package.Version));

            var matchingFiles = packageFiles.
                Where   (it => it?.TargetFramework != null).
                Where   (it => SameFrameworkVersion(p, it)).
                Select  (it => new FileInfo(Path.Combine(packagePath, it.Path))).ToList();

            Ensure(targetDirectory);

            matchingFiles.ForEach(it => it.CopyTo(Path.Combine(targetDirectory.FullName, it.Name), true));

            return matchingFiles;
        }

        private static FrameworkName[] From(IPackage thirdPartyVersions)
        {
            return thirdPartyVersions.GetSupportedFrameworks().Select(thirdParty =>
            {
                if (thirdParty.Version > new Version())
                    return thirdParty;

                var match = Regex.Match(thirdParty.Profile, @"net(?<version>[\d]+)");

                if (match.Success)
                {
                    return new FrameworkName(thirdParty.Identifier, new Version(string.Join(".", match.Groups["version"].Value.ToCharArray())), thirdParty.Profile);
                }

                throw new Exception($"Unable to determine supported framework from {thirdParty}");

            }).ToArray();
        }

        private static bool SameFrameworkVersion(NugetPackage package, IPackageFile thirdParty)
        {
            var targetFramework = thirdParty.TargetFramework;

            if (targetFramework.Version > new Version())
                return package.Matches(targetFramework.Version);

            var match = Regex.Match(thirdParty.TargetFramework.Profile, @"net(?<version>[\d]+)");

            if (match.Success)
            {
                return package.Matches(new Version(string.Join(".", match.Groups["version"].Value.ToCharArray())));
            }

            throw new Exception($"Unable to determine the version described by {thirdParty}");
        }

        private static void Ensure(DirectoryInfo targetDirectory)
        {
            if (false == targetDirectory.Exists)
            {
                targetDirectory.Create();
            }
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
