using Conduit.UseCases.Semver.Semver;

namespace Conduit.UseCases.Semver.Assemblies
{
    public static class AssemblyVersion
    {
        public static SemVersion For(string filename)
        {
            return Private.Version.For.File (filename, "AssemblyVersion");
        }

        public static void BumpMinor(params string[] filenames)
        {
            foreach(var filename in filenames) 
            {
                Private.AssemblyInfoVersion.BumpMinor(filename, "AssemblyVersion");
                Private.AssemblyInfoVersion.BumpMinor(filename, "AssemblyFileVersion");
            }
        }

        public static void BumpMajor(string filename)
        {
            Private.AssemblyInfoVersion.BumpMajor(filename, "AssemblyVersion");
            Private.AssemblyInfoVersion.BumpMajor(filename, "AssemblyFileVersion");
        }

        public static void BumpPatch(string[] filenames)
        {
            foreach (var filename in filenames)
            {
                Private.AssemblyInfoVersion.BumpPatch(filename, "AssemblyVersion");
                Private.AssemblyInfoVersion.BumpPatch(filename, "AssemblyFileVersion");
            }
        }
    }

}