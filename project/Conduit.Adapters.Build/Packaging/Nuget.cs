﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Conduit.Lang;
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

            matchingFiles.ForEach(it => it.CopyTo(Path.Combine(targetDirectory.FullName, it.Name)));

            return matchingFiles;
        }

        private static bool SameFrameworkVersion(NugetPackage package, IPackageFile thirdParty)
        {
            // @todo: duplicated at <FrameworkVersion.ctor>
            var value = string.Join("", thirdParty.TargetFramework.Version.ToString().Split('.'));

            var actual = new FrameworkVersionName($"net{value}");

            return actual.Value.Equals(package.FrameworkVersion.Name, StringComparison.InvariantCultureIgnoreCase);
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

    public class NugetPackage
    {
        public string Id { get; }
        public PackageVersion Version { get; }
        public FrameworkVersion FrameworkVersion { get; private set; }

        public NugetPackage(string id) : this(id, null, null)
        {
        }

        public NugetPackage(string id, PackageVersion version, FrameworkVersion frameworkVersion)
        {
            Id = id;
            Version = version;
            FrameworkVersion = frameworkVersion;
        }

        public NugetPackage With(FrameworkVersion frameworkVersion)
        {
            return ((NugetPackage) MemberwiseClone()).Tap(it => it.FrameworkVersion = frameworkVersion);
        }
    }

    public class PackageVersion
    {
        public string Value { get; private set; }

        public PackageVersion(string value)
        {
            Value = value;
        }
    }

    public static class PackagesConfig
    {
        public static List<NugetPackage> Read(FileInfo file)
        {
            return new PackageReferenceFile(file.FullName).GetPackageReferences().Select(it => 
                new NugetPackage(it.Id, new PackageVersion(it.Version.ToString()), FrameworkVersion.Net45)).ToList();
        }
    }
}
