using System;

namespace Conduit.Adapters.Build.IO
{
    public static class Assembly
    {
        public static string NewestMatching(Action<string> log, string pattern)
        {
            log($"TestGlob: {pattern}");

            var fileInfo = Dir.Newest(new Glob(pattern));

            if (null == fileInfo)
                throw new Exception($"Unable to find any assemblies matching pattern <{pattern}>");

            return fileInfo.FullName;
        }
    }
}
