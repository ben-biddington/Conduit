using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.Lang;
using NuGet;

namespace Conduit.Adapters.Build.Packaging.Private
{
    internal static class Flatten
    {
        internal static List<FileInfo> Apply(Uri uri, DirectoryInfo packageDirectory, DirectoryInfo targetDirectory, params NugetPackage[] packages)
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
            var packageFiles = Files(uri, packageDirectory, p.Id);

            var packagePath = Path.Combine(packageDirectory.FullName, string.Join(".", p.Id, p.Version));

            var matchingFiles = packageFiles.
                Where(it => it?.TargetFramework != null).
                Where(it => MatchingFrameworkVersion(p, it)).
                Select(it => new FileInfo(Path.Combine(packagePath, it.Path))).ToList();

            Ensure(targetDirectory);

            return matchingFiles.Tap(it => it.ForEach(f => f.CopyTo(Path.Combine(targetDirectory.FullName, f.Name), true)));
        }

        private static List<IPackageFile> Files(Uri uri, DirectoryInfo packageDirectory, string id)
        {
            var package = new PackageManager(PackageRepository(uri), packageDirectory.FullName).LocalRepository.FindPackage(id);

            return package?.GetFiles().ToList() ?? new List<IPackageFile>(0);
        }

        private static bool MatchingFrameworkVersion(NugetPackage package, IPackageFile thirdParty)
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

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }
    }
}
