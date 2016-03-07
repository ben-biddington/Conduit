using System;
using System.Runtime.Versioning;

namespace Conduit.Adapters.Build.Packaging
{
    public static class FrameworkNames
    {
        public static readonly FrameworkName Net45 = new FrameworkName(".NETFramework", new Version("4.5"));
        public static readonly FrameworkName Net40 = new FrameworkName(".NETFramework", new Version("4.0"));
    }
}