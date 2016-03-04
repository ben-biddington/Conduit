using Conduit.Lang;

namespace Conduit.Adapters.Build.Packaging
{
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
}