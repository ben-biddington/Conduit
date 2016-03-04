using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    public static class PackagesConfig
    {
        public static List<NugetPackage> Read(FileInfo file)
        {
            return new PackageReferenceFile(file.FullName).GetPackageReferences().Select(it => 
                new NugetPackage(it.Id, new PackageVersion(it.Version.ToString()), FrameworkVersion.Net45)).ToList();
        }
    }
}