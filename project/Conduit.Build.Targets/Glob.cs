using System.Linq;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Conduit.Adapters.Build;
using Conduit.Adapters.Build.IO;

namespace Conduit.Build.Targets
{
    public class Glob : Task
    {
        [Required]
        public string[] Pattern { get; set; }
       
        [Output]
        public string Path { get; private set;}

        public override bool Execute()
        {
            Path = Dir.Newest((Pattern ?? new string[0]).Select(it => new Adapters.Build.IO.Glob(it)).ToArray())?.FullName;

            return true;
        }
    }
}

