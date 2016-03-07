using System;
using System.Linq;
using System.Runtime.Versioning;
using Conduit.Lang;

namespace Conduit.Adapters.Build.Packaging
{
    public class NugetPackage
    {
        public string Id { get; }
        public PackageVersion Version { get; }
        public FrameworkName[] FrameworkNames { get; private set; }

        public NugetPackage(string id) : this(id, null, null)
        {
        }

        public NugetPackage(string id, PackageVersion version, params FrameworkName[] frameworkNameses)
        {
            Id = id;
            Version = version;
            FrameworkNames = frameworkNameses;
        }

        public NugetPackage With(FrameworkName frameworkName)
        {
            return ((NugetPackage) MemberwiseClone()).Tap(it => it.FrameworkNames = new[] { frameworkName });
        }

        public bool Matches(Version version)
        {
            return FrameworkNames.Any(it => it.Version >= version);
        }
    }
}