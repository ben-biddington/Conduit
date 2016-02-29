using Conduit.UseCases.Semver.Semver;

namespace Conduit.UseCases.Semver.Assemblies
{
    public static class AssemblyVersion
    {
        public static SemVersion For(string filename)
        {
            return Conduit.UseCases.Semver.Assemblies.Private.Version.For.File (filename, "AssemblyVersion");
        }

        public static void BumpMinor(params string[] filenames)
        {
            foreach(var filename in filenames) 
            {
                Conduit.UseCases.Semver.Assemblies.Private.AssemblyInfoVersion.BumpMinor(filename, "AssemblyVersion");
                Conduit.UseCases.Semver.Assemblies.Private.AssemblyInfoVersion.BumpMinor(filename, "AssemblyFileVersion");
            }
        }

        public static void BumpMajor(string filename)
        {
            Conduit.UseCases.Semver.Assemblies.Private.AssemblyInfoVersion.BumpMajor(filename, "AssemblyVersion");
            Conduit.UseCases.Semver.Assemblies.Private.AssemblyInfoVersion.BumpMajor(filename, "AssemblyFileVersion");
        }
    }

}