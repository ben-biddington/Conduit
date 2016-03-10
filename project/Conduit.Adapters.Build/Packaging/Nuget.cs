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
            Install(uri, directory, _ => { }, new InstallOptions(), packages);
        }

        public static void Install(Uri uri, DirectoryInfo directory, Action<string> log, params NugetPackage[] packages)
        {
            Install (uri, directory, log, new InstallOptions(), packages);
        }

        public static void Install(Uri uri, DirectoryInfo directory, Action<string> log, InstallOptions opts, params NugetPackage[] packages)
        {
            var packageManager = new PackageManager(PackageRepository(uri), directory.FullName); //@todo: parallelize

            foreach (var package in packages)
            {
                var semanticVersion = package.Version != null ? SemanticVersion.Parse(package.Version.Value) : null;

                log($"Installing package <{package.Id}, {semanticVersion.Version}> " + (opts.IncludeDependencies ? "with dependencies" : "without dependencies"));

                packageManager.PackageInstalling    += (_, e) => { log($"Installing package <{e.Package.Id}, {e.Package.Version}>"); };
                packageManager.PackageInstalled     += (_, e) => { log($"Installed package <{e.Package.Id}, {e.Package.Version}>"); };

                packageManager.InstallPackage(package.Id, semanticVersion, false == opts.IncludeDependencies, false);

                log($"Installed package <{package.Id}, {semanticVersion.Version}> ");
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
