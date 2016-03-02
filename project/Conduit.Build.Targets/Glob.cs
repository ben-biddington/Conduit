using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Conduit.Adapters.Build;

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
            Path = Dir.Newest(new Adapters.Build.Glob(Pattern[0])).FullName;

            return true;
        }
    }
}

