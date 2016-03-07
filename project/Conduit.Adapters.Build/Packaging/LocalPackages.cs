using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
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
            return new NugetPackage(thirdParty.Id, new PackageVersion(thirdParty.Version.ToString()), From(thirdParty));
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

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepositoryFactory.Default.CreateRepository(uri.AbsoluteUri);
        }
    }
}