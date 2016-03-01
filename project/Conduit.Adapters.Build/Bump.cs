using System;
using System.Linq;
using Conduit.UseCases.Semver.Assemblies;

namespace Conduit.Adapters.Build
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

