using System;
using System.IO;
using System.Linq;
using Minimatch;

namespace Conduit.Adapters.Build
{
    public class Dir
    {
        public static FileInfo Newest(Filename filename)
        {
            return Newest(new Glob($@".\**\{filename.Value}"));
        }

        public static FileInfo Newest(Glob glob)
        {
            var all = Directory.GetFiles(".", "*", SearchOption.AllDirectories);

            var newest = Minimatcher.
                Filter(all, glob.Value, new Options {AllowWindowsPaths = true}).
                Select(it => new FileInfo(it)).
                ToList();

            return false == newest.Any()
                ? null
                : newest.OrderBy(it => it.LastWriteTimeUtc).LastOrDefault();
        }
    }

    public class Filename
    {
        public string Value { get; }

        public Filename(string file) : this(new FileInfo(file))
        {}

        public Filename(FileInfo file)
        {
            Value = file.Name;
        }
    }

    public class Glob
    {
        public string Value { get; }

        public Glob(string pattern)
        {
            var invalidCharacters = pattern.Select(x => x).Intersect(Path.GetInvalidPathChars()).ToList();

            if(invalidCharacters.Any())
                throw new Exception($"These characters are considered invalid: {string.Join(",", invalidCharacters)}");

            Value = pattern;
        }
    }
}