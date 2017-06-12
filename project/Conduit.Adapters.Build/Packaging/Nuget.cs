using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            Install(uri, directory, new InstallOptions(), packages);
        }

        public static void Install(Uri uri, DirectoryInfo directory, InstallOptions opts, params NugetPackage[] packages)
        {
            Install(uri, directory, _ => { }, opts, packages);
        }

        public static void Install(Uri uri, DirectoryInfo directory, Action<string> log, params NugetPackage[] packages)
        {
            Install(uri, directory, log, new InstallOptions(), packages);
        }

        public static void Install(Uri uri, DirectoryInfo directory, Action<string> log, InstallOptions opts, params NugetPackage[] packages)
        {
            var packageManager = new PackageManager(PackageRepository(uri, log), directory.FullName); //@todo: parallelize

            log($"Installing packages from source server <{uri}> ({packageManager.SourceRepository.Source})");

            foreach (var package in packages)
            {
                var semanticVersion = package.Version != null ? SemanticVersion.Parse(package.Version.Value) : null;

                log($"Installing package <{package.Id}, {semanticVersion.Version}> " + (opts.IncludeDependencies ? "with dependencies" : "without dependencies"));

                packageManager.PackageInstalling    += (_, e) => { log($"Installing package <{e.Package.Id}, {e.Package.Version}>"); };
                packageManager.PackageInstalled     += (_, e) => { log($"Installed package <{e.Package.Id}, {e.Package.Version}>"); };

                try
                {
                    packageManager.InstallPackage(package.Id, semanticVersion, false == opts.IncludeDependencies, false);
                }
                catch (WebException e)
                {
                    throw new Exception(
                        $"Failed to install package <{package.Id}>. The URL in question is <{e.Response?.ResponseUri}>. " +
                        $"See inner exception for details.", e);
                }

                log($"Installed package <{package.Id}, {semanticVersion.Version}>");
            }
        }

        private static IPackageRepository PackageRepository(Uri uri)
        {
            return PackageRepository(uri, m => { });
        }

        private static IPackageRepository PackageRepository(Uri uri, Action<string> log)
        {
            var factory = new PackageRepositoryFactory
            {
                HttpClientFactory = url => HttpClient(url, log)
            };

            return factory.CreateRepository(uri.AbsoluteUri);
        }

        private static IHttpClient HttpClient(Uri url, Action<string> log)
            => new DefaultHttpClient(url, DefaultHttpClient.Options.Default.With(log));

        public class InstallOptions
        {
            public bool IncludeDependencies { get; private set; }
            public bool BypassProxy { get; private set; }

            public InstallOptions(bool dependencies = false, bool bypassProxy = false)
            {
                IncludeDependencies = dependencies;
                BypassProxy = bypassProxy;
            }
        }
    }
}
