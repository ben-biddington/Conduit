using System.IO;

namespace Conduit.Adapters.Build.Packaging
{
    public static class Nuget
    {
        public static FileInfo[] Flatten(DirectoryInfo packageDirectory, FrameworkVersion version, DirectoryInfo targetDirectory)
        {
            return targetDirectory.GetFiles();
        }
    }
}
