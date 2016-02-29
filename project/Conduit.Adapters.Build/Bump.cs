using System;
using Conduit.UseCases.Semver.Assemblies;

namespace Conduit.Adapters.Build
{
    public static class Bump
    {
        public static void  Minor()
        {
            AssemblyVersion.BumpMajor("AssemblyInfo.cs");
        }
    }
}

