using System;
using System.IO;
using System.Linq;

namespace Conduit.Adapters.Build
{
    public class Dir
    {
        public static FileInfo Newest(string filenameWithExtension)
        {
            var newest = Directory.GetFiles(Environment.CurrentDirectory, $"*{filenameWithExtension}", SearchOption.AllDirectories).Select(it => new FileInfo(it)).ToList();

            return false == newest.Any() 
                ? null 
                : newest.OrderBy(it => it.LastWriteTimeUtc).LastOrDefault();
        }
    }
}