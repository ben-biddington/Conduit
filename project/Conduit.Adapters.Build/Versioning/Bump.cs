using System.Linq;
using Conduit.Adapters.Build.IO;
using Conduit.UseCases.Semver.Assemblies;

namespace Conduit.Adapters.Build.Versioning
{
    public static class Bump
    {
        public static void Minor()
        {
            var allAssemblyInfo = Dir.All (new Filename ("AssemblyInfo.cs"));

            AssemblyVersion.BumpMinor(allAssemblyInfo.Select(it => it.FullName).ToArray());
        }

        public static void Patch()
        {
            var allAssemblyInfo = Dir.All(new Filename("AssemblyInfo.cs"));

            AssemblyVersion.BumpPatch(allAssemblyInfo.Select(it => it.FullName).ToArray());
        }
    }
}

